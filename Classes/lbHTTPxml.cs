////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using static LocalBitcoinsAPI.lbHTTPCore;

namespace LocalBitcoinsAPI
{
    class lbHTTPxml : lbMetadata
    {
        #region fields
        protected string baseurl = "https://localbitcoins.com";
        protected string my_hmac_auth_key;
        protected string my_hmac_auth_secret;
        //
        protected string unixTimestamp;
        protected string message;
        protected string signature;
        #endregion

        /// <summary>
        /// Отправить запрос серверу
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="_params">Параметры POST/GET запроса</param>
        /// <param name="method">Имя метода отправки запроса (POST или GET)</param>
        /// <returns></returns>
        private string sendRequest(string endpoint, Dictionary<string, string> _params, string method)
        {
            method = method.ToLower();
            using (WebClient client = new WebClient())
            {
                WebProxy wp = new WebProxy("127.0.0.1:9150");
                client.Proxy = wp;
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                NameValueCollection reqparm = new NameValueCollection();
                unixTimestamp = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();
            }
            /*
            using (HttpRequest request = new HttpRequest())
            {
                request.Proxy = Socks5ProxyClient.Parse("127.0.0.1:9150");
                RequestParams urlParams = new RequestParams();
                if (_params != null && _params.Count > 0)
                    urlParams.AddRange(_params);

                
                message = unixTimestamp + my_hmac_auth_key + endpoint;
                signature = HashHMAC(my_hmac_auth_secret, message);
                request.AddHeader("Apiauth-Key", my_hmac_auth_key);
                request.AddHeader("Apiauth-Nonce", unixTimestamp);
                request.AddHeader("Apiauth-Signature", signature);
                // Отправляем запрос.
                HttpResponse response;
                if (method == "get")
                    response = request.Get(baseurl + endpoint, urlParams);
                else
                {
                    if (urlParams.Count > 0)
                        response = request.Post(baseurl + endpoint, urlParams);
                    else
                        response = request.Post(baseurl + endpoint);
                }
                
            // Принимаем тело сообщения в виде строки.
            return response.ToString();
            }
            */
            return "";
        }

        /////////////////////////////////////////////////
        #region Public functions
        protected override string getPlaces_as_string(string lat, string lon, string countrycode = "", string location_string = "")
        {
            Dictionary<string, string> _params = new Dictionary<string, string>() { { "lat", lat }, { "lon", lon } };
            if (countrycode != "")
                _params.Add("countrycode", countrycode);
            if (location_string != "")
                _params.Add("location_string", location_string);
            return sendRequest("/api/places/", _params, "post");
        }

        protected override string getLocalAdLookups_as_string(string ad_type, string location_id, string location_slug)
        {
            return sendRequest("/" + ad_type + "-bitcoins-" + (ad_type == "buy" ? "with" : "for") + "-cash/" + location_id + "/" + location_slug + "/", null, "get");
        }

        /// <summary>
        /// Список действительных и признанных валют декретных для LocalBitcoins.com.
        /// </summary>
        /// <returns></returns>
        protected string getCurrencies_as_string()
        {
            return sendRequest("/api/currencies/", null, "get");
        }

        /// <summary>
        /// Список действительных и признанных countrycodes для LocalBitcoins.com. 
        /// </summary>
        /// <returns>Возвращаемое значение структурирована следующим образом:
        /// {
        ///     "data": {
        ///         "cc_list": ["fi", "fr", "us", ...],
        ///         "cc_count": 50
        ///     }
        /// }
        /// </returns>
        protected string getCountrycodes_as_string()
        {
            return sendRequest("/api/countrycodes/", null, "get");
        }

        /// <summary>
        /// Получить доступные методы оплаты
        /// </summary>
        /// <param name="countrycode">Фильтр: Код страны</param>
        /// <returns></returns>
        protected string getPaymentMethods_as_string(string countrycode = "")
        {
            if (countrycode != "")
                countrycode = countrycode + "/";

            return sendRequest("/api/payment_methods/" + countrycode, null, "get");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Получить рекламные объявления онлайн покупки/продажи. Сумма это максимальная сумма, доступная для запроса торговли. Цена основана на уравнении цены и комиссии % введенной автором объявления
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <returns></returns>
        protected string getOrderbook_as_string(string currency)
        {
            return sendRequest("/bitcoincharts/" + currency + "/orderbook.json", null, "get");
        }

        /// <summary>
        /// Получить все закрытые сделки по валюте. Обновляется каждые 15 минут
        /// Максимальный размер выборки 500 записей. Используйте ?since=parameter для сдвига среза данных
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <returns></returns>
        protected string getTrades_as_string(string currency)
        {
            return sendRequest("/bitcoincharts/" + currency + "/trades.json", null, "get");
        }

        /// <summary>
        /// Средняя цена биткоина по всем валютам
        /// Если по какой либо валюте не достаточно информации для вычисления средней цены, то валюта не отображается
        /// </summary>
        /// <returns></returns>
        protected string getTickerAllCurrencies_as_string()
        {
            return sendRequest("/bitcoinaverage/ticker-all-currencies/", null, "get");
        }
        #endregion

        /////////////////////////////////////////////////
        #region Get functions
        //////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////
        //
        /// <summary>
        /// Возвращает информацию о профиле пользователя
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>JSON данные пользователя</returns>
        /// {
        /// "data": {
        /// "username": "bitcoinbaron",
        /// "created_at": "2013-06-25T14:11:30+00:00",
        /// "trading_partners_count": 0,
        /// "feedbacks_unconfirmed_count": 0,
        /// "trade_volume_text": "Less than 25 BTC",
        /// "has_common_trades": false,
        /// "confirmed_trade_count_text": "0",
        /// "blocked_count": 0,
        /// "feedback_score": 0,
        /// "feedback_count": 0,
        /// "url": "https://localbitcoins.com/p/bitcoinbaron/",
        /// "trusted_count": 0,
        /// "identity_verified_at": null or date,
        /// "real_name_verifications_trusted": 1,
        /// "real_name_verifications_untrusted": 4,
        /// "real_name_verifications_rejected": 0
        ///         }
        /// }
        protected string getAccountInfo_as_string(string username)
        {
            return sendRequest("/api/account_info/" + username + "/", null, "get");
        }

        /// <summary>
        /// Возвращает информацию текущего авторизованного пользователя (владелец маркера аутентификации).
        /// </summary>
        /// <returns>JSON данные</returns>
        /// {
        /// "data": {
        /// "username": "bitcoinbaron",
        /// "created_at": "2013-06-25T14:11:30+00:00",
        /// "trading_partners_count": 0,
        /// "feedbacks_unconfirmed_count": 0,
        /// "trade_volume_text": "Less than 25 BTC",
        /// "has_common_trades": false,
        /// "confirmed_trade_count_text": "0",
        /// "blocked_count": 0,
        /// "feedback_score": 0,
        /// "feedback_count": 0,
        /// "url": "https://localbitcoins.com/p/bitcoinbaron/",
        /// "trusted_count": 0,
        /// "identity_verified_at": null or date,
        /// "real_name_verifications_trusted": 1,
        /// "real_name_verifications_untrusted": 4,
        /// "real_name_verifications_rejected": 0
        ///         }
        /// }
        protected string getMyself_as_string()
        {
            return sendRequest("/api/myself/", null, "get");
        }

        /// <summary>
        /// Получить контакты
        /// </summary>
        /// <param name="state">Статус контакта</param>
        /// <param name="sub_filter">Фильтр контакта</param>
        /// <returns></returns>
        /// Возвращается список контактов, попадающих под фильтры
        /// Каждый элемент списка представляет из себя структуру:
        /// {
        /// "data": {
        /// "created_at": "2013-12-06T15:23:01.61",
        /// "buyer": {
        /// "username": "hylje",
        /// "trade_count": "30+",
        /// "feedback_score": "100",
        /// "name": "hylje (30+; 100)",
        /// "last_online": "2013-12-19T08:28:16+00:00",
        /// "real_name": string or null if ONLINE trade where you are the seller,
        /// "company_name": string or null if ONLINE trade where you are the seller,
        /// "real_name_verifiers": [{"username": "henu.tester", "verified_at": "2016-10-13T13:49:45+00:00"}] if ONLINE trade where you are the seller,
        /// "countrycode_by_ip": string or null if ONLINE trade where you are the seller,
        /// "countrycode_by_phone_number": string or null if ONLINE trade where you are the seller
        /// }
        /// "seller": {
        /// "username": "jeremias",
        /// "trade_count": "100+",
        /// "feedback_score": "100",
        /// "name": "jeremias (100+; 100)",
        /// "last_online": "2013-12-19T06:28:51+00:00"
        /// }
        /// "reference_code": "123",
        /// "currency": "EUR",
        /// "amount": "105.55",
        /// "amount_btc": "190",
        /// "fee_btc": "1.9",
        /// "exchange_rate_updated_at": "2013-06-20T15:23:01+00:00",
        /// "advertisement": {
        /// "id": 123,
        /// "trade_type": "ONLINE_SELL"
        /// "advertiser": {
        /// "username": "jeremias",
        /// "trade_count": "100+",
        /// "feedback_score": "100",
        /// "name": "jeremias (100+; 100)",
        /// "last_online": "2013-12-19T06:28:51.604754+00:00"
        /// }
        /// },
        /// "contact_id": 1234
        /// "canceled_at": null,
        /// "escrowed_at": "2013-12-06T15:23:01+00:00",
        /// "funded_at": "2013-12-06T15:23:01+00:00",
        /// "payment_completed_at": "2013-12-06T15:23:01+00:00",
        /// "disputed_at": null,
        /// "closed_at": null,
        /// "released_at": null,
        /// "is_buying": true,
        /// "is_selling": false,
        /// "account_details": ! see below,
        /// "account_info": Payment details of ONLINE_SELL as string, if account_details is missing.,
        /// "floating": boolean if LOCAL_SELL
        /// },
        /// "actions": {
        /// "mark_as_paid_url": "/api/contact_mark_as_paid/1/",
        /// "advertisement_public_view": "/ads/123",
        /// "message_url": "/api/contact_messages/1234",
        /// "message_post_url": "/api/contact_message_post/1234"
        ///             }
        /// }
        protected string getDashboard_as_string(DashboardStates state = DashboardStates.Open, DashboardFilter sub_filter = DashboardFilter.All)
        {
            return sendRequest("/api/dashboard/" + (state == DashboardStates.Open ? "" : Enum.GetName(typeof(DashboardStates), state).ToLower() + "/") + (sub_filter == DashboardFilter.All ? "" : Enum.GetName(typeof(DashboardFilter), sub_filter).ToLower() + "/"), null, "get");
        }

        /// <summary>
        /// Читает все переданые сообщения контакта в виде списка
        /// В случае успеха возвращается положительное сообщение
        /// attachment_* поля существуют, только если есть вложение.
        /// </summary>
        /// <param name="contact_id"></param>
        /// <returns></returns>
        /// Структура элемента списка (сообщения) выглядит следующим образом:
        /// {
        /// "msg": "message body",
        /// "sender":
        ///     {
        ///     "id": 123,
        ///     "name": "bitcoinbaron (0)",
        ///     "username": "bitcoinbaron",
        ///     "trade_count": 0,
        ///     "last_online": "2013-12-17T03:31:12.382862+00:00"
        ///     },
        /// "created_at": "2013-12-19T16:03:38.218039",
        /// "is_admin": false,
        /// "attachment_name": "cnXvo5sV6eH4-some_image.jpg",
        /// "attachment_type": "image/jpeg",
        /// "attachment_url": "https://localbitcoins.com/api/..."
        /// }
        protected string getContactMessages_as_string(string contact_id)
        {
            return sendRequest("/api/contact_messages/" + contact_id + "/", null, "get");
        }

        /// <summary>
        /// Получить информацию о контакте. Те же поля, как и в /api/contacts/.
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <returns></returns>
        protected string getContactInfo_as_string(string contact_id)
        {
            return sendRequest("/api/contact_info/" + contact_id + "/", null, "get");
        }

        /// <summary>
        /// Kонтакты разделенные запятыми в виде списка список контактных идентификаторов, которые вы хотите получить доступ.
        /// Маркер владелец должен быть продавцом или покупателем в контактах. Контакты, которые не проходят эту проверку просто не возвращаются.
        /// запрошено может быть не более 50 контактов.
        /// Контакты будут возвращены в случайном порядке.
        /// </summary>
        /// <param name="contacts">Идентификаторы контактов</param>
        /// <returns></returns>
        protected string getContactsInfo_as_string(string contacts)
        {
            return sendRequest("/api/contact_info/", new Dictionary<string, string>() { { "contacts", contacts } }, "get");
        }

        /// <summary>
        /// Возвращает максимум 50 новых торговых сообщений. Сообщения отсортированы по времени отправления, а также новейший является первым. Список имеет тот же формат, как /api/contact_messages/, но каждое сообщение также имеет поле contact_id.
        /// Если вы хотели бы видеть старые сообщения, используйте параметр before. Он принимает дату в формате UTC ISO 8601.
        /// </summary>
        /// <param name="before">Дата в формате UTC ISO 8601. Если указать этот параметр, то возвращены будут только сообщения до указаной даты</param>
        /// <returns></returns>
        protected string getRecentMessages_as_string(string before = "")
        {
            return sendRequest("/api/recent_messages/", (before == "" ? null : new Dictionary<string, string>() { { "before", before } }), "get");
        }

        /// <summary>
        /// Получает информацию о балансе бумажника токена владельца.
        /// </summary>
        /// <returns></returns>
        /// {
        /// "message": "OK",
        /// "total": {
        ///   "balance": "0.05",
        ///   "sendable": "0.05"
        ///   },
        /// "sent_transactions_30d": [
        ///   {"txid": ...
        ///    "amount": "0.05",
        ///    "description": "Internal send",
        ///    "tx_type": 5,
        ///    "created_at": "2013-12-20T15:27:36+00:00"
        ///   }
        /// ],
        /// "received_transactions_30d": [...],
        /// "receiving_address_count": 1,
        /// "receiving_address_list": [{
        ///   "address": "15HfUY9LwwewaWwrKRXzE91tjDnHmye1hc",
        ///   "received": "0.0"
        ///   }]
        ///    }
        ///    Списки транзакций являются за последние 30 дней. Там нет LocalBitcoins API для извлечения старых транзакций, чем это.
        ///    
        ///  типы транзакций
        ///  Операции в бумажнике API есть несколько типов.Все полученные транзакции типа 3.
        ///  1	Отправлено to_address: адрес получателя Bitcoin
        ///  2	В ожидании Отправить. Процессор транзакций еще не опубликовал транзакцию, содержащую это послать к сети Bitcoin, но будет делать это как можно быстрее..to_address: the to-be-recipient Bitcoin address
        ///  3	Прочие операции, e.g.received сделки. Смотрите поле описания
        ///  4	Плата сети Bitcoin
        ///  5	Внутренний перевод на кошелёк LocalBitcoins
        protected string getWallet_as_string()
        {
            return sendRequest("/api/wallet/", null, "get");
        }

        /// <summary>
        /// То же, что /api/walet/, но возвращает receiving_address_list со всеми полями.
        /// (Там же receiving_address_count но это всегда 1:. Только последний принимающий адрес никогда не возвращается к этому вызову)
        /// Используйте это вместо того, чтобы, если вы не заботитесь о сделках на данный момент.
        /// </summary>
        /// <returns></returns>
        protected string getWalletBallance_as_string()
        {
            return sendRequest("/api/wallet-balance/", null, "get");
        }

        /// <summary>
        /// Получает неиспользуемый адрес для приема.
        /// Обратите внимание, что этот API может постоянно возвращаться один и тот же (не используемый) адрес при повторном вызове.
        /// </summary>
        /// <returns></returns>
        protected string getWalletAddress_as_string()
        {
            return sendRequest("/api/wallet-addr/", null, "post");
        }

        /// <summary>
        /// Список объявлений (можно накладывать фильтр)
        /// https://localbitcoins.com/api-docs/#api_toc23
        /// </summary>
        /// <returns></returns>
        protected string getOwnAds_as_string(LB_TradeVisibleStatus visible = LB_TradeVisibleStatus.All, LB_TradeTypes trade_type = LB_TradeTypes.ALL, string currency = "", string countrycode = "")
        {
            Dictionary<string, string> _params = new Dictionary<string, string>();
            if (visible != LB_TradeVisibleStatus.All)
                _params.Add("visible", visible.ToString());
            if (trade_type != LB_TradeTypes.ALL)
                _params.Add("trade_type", Enum.GetName(typeof(LB_TradeTypes), trade_type));
            if (currency != "")
                _params.Add("currency", currency);
            if (countrycode != "")
                _params.Add("countrycode", countrycode);

            return sendRequest("/api/ads/", _params, "post");
        }

        /// <summary>
        /// Получает объявление по ID.
        /// Если объявление найдено, возвращает в виде списка с одним единственным элементом.
        /// Тем не менее, возвращает ошибку, если объявление не доступно по какой-либо причине, 
        /// вместо возвращения пустого списка. 
        /// </summary>
        /// <param name="ad_id">Идентефикатор объявления</param>
        /// <returns></returns>
        protected string getAd_as_string(string ad_id)
        {
            return sendRequest("/api/ad-get/" + ad_id + "/", null, "get");
        }

        /// <summary>
        /// Получает все объявления из списка идентификаторов объявлений (разделенных запятыми).
        /// Объявления, которые не найдены просто не будут добалвены в список.
        /// Макс 50 объявлений за один раз.
        /// Объявления не упорядочены.
        /// </summary>
        /// <param name="ad_ids">Идентификаторы объявлений (разделенные запятыми)</param>
        /// <returns></returns>
        protected string getAds_as_string(string[] ad_ids)
        {
            return sendRequest("/api/ad-get/?ads=" + String.Join(",", ad_ids), null, "get");
        }
        //
        //////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////
        #endregion

        /////////////////////////////////////////////////
        #region Set functions

        /// <summary>
        /// Освобождает депонирование контакта, указанного идентификатором {contact_id}.
        /// В случае успеха возвращается положительное сообщение.
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <returns></returns>
        protected string setContactRelease_as_string(string contact_id)
        {
            return sendRequest("/api/contact_release/" + contact_id + "/", null, "post");
        }

        /// <summary>
        /// Освобождает депонирование контакта, указанного идентификатором {contact_id}.
        /// В случае успеха возвращается положительное сообщение.
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <param name="pincode">PIN - код</param>
        /// <returns></returns>
        protected string setContactReleasePin_as_string(string contact_id, string pincode)
        {
            return sendRequest("/api/contact_release_pin/" + contact_id + "/", new Dictionary<string, string>() { { "pincode", pincode } }, "post");
        }

        /// <summary>
        /// Отметить контакт как "Оплаченый"
        /// It is recommended to access this API through /api/online_buy_contacts/ entries' action key.
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <returns></returns>
        protected string setMarkContactAsPaid_as_string(string contact_id)
        {
            return sendRequest("/api/contact_mark_as_paid/" + contact_id + "/", null, "get");
        }

        /// <summary>
        /// Отменяет контакт, если это возможно
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <returns></returns>
        protected string setCancelContact_as_string(string contact_id)
        {
            return sendRequest("/api/contact_cancel/" + contact_id + "/", null, "post");
        }

        /// <summary>
        /// Попытаться оплатить контакт из кошелька продавца.
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <returns></returns>
        protected string setFundContact_as_string(string contact_id)
        {
            return sendRequest("/api/contact_fund/" + contact_id + "/", null, "post");
        }

        #endregion

        /////////////////////////////////////////////////
        #region Other functions

        /// <summary>
        /// Удалить объявление
        /// </summary>
        /// <param name="ad_id">Идентификатор объявления</param>
        /// <returns></returns>
        protected string deleteAD_as_string(string ad_id)
        {
            return sendRequest("/api/ad-delete/" + ad_id + "/", null, "post");
        }

        /// <summary>
        /// Проверяет переданный PIN-код. Активный или нет
        /// Вы можете использовать этот метод, чтобы убедиться, человек, использующий сессии является законным пользователем.
        /// </summary>
        /// <param name="code">PIN-код</param>
        /// <returns></returns>
        protected string checkPinCode_as_string(string code)
        {
            return sendRequest("/api/pincode/", new Dictionary<string, string>() { { "code", code } }, "post");
        }

        /// <summary>
        /// Отправить сообщение в контакт
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <param name="message">Текст сообщения</param>
        /// <returns></returns>
        protected string postMessageToContact_as_string(string contact_id, string message)
        {
            return sendRequest("/api/contact_message_post/" + contact_id + "/", new Dictionary<string, string>() { { "msg", message } }, "post");
        }

        /// <summary>
        /// Начать спор с контактом, если это возможно.
        /// Можно дать краткое описание, используя параметр "topic". Это может помоч поддержке для решения проблемы.
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <param name="topic">Заголовок диспута</param>
        /// <returns></returns>
        protected string startDispute_as_string(string contact_id, string topic = "")
        {
            return sendRequest("/api/contact_dispute/" + contact_id + "/", (topic == "" ? null : new Dictionary<string, string>() { { "topic", topic } }), "post");
        }

        /// <summary>
        /// Попытка создать контакт для торговли Bitcoins.
        /// ammount - является числом в декретных валюте объявления.
        /// Возвращает URL API для вновь созданного контакта на actions.contact_url.
        /// Если контакт был в состоянии финансировать автоматически указывается на data.funded.
        /// Only non-floating LOCAL_SELL may return unfunded, all other trade types either fund or fail.
        /// </summary>
        /// <param name="contact_id">Идентификатор контакта</param>
        /// <returns></returns>
        protected string createContact_as_string(string contact_id, string ammount, string message = "")
        {
            Dictionary<string, string> _params = new Dictionary<string, string>() { { "ammount", ammount } };
            if (!string.IsNullOrEmpty(message))
                _params.Add("message", message);
            return sendRequest("/api/contact_create/" + contact_id + "/", _params, "post");
        }

        /// <summary>
        /// Дает обратную связь с пользователем. 
        /// Возможные значения обратной связи являются (в виде строки): trust, positive, neutral, block, block_without_feedback.
        /// Вы можете также установить сообщение обратной связи, используя поле Сообщ с некоторыми исключениями.
        /// Обратная связь block_without_feedback очищает сообщение и с блоком сообщение является обязательным.
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="feedback">Режим обратной связи</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        protected string postFeedbackToUser_as_string(string username, FeedbackStatus feedback, string message = "")
        {
            Dictionary<string, string> _params = new Dictionary<string, string>() { { "feedback", Enum.GetName(typeof(FeedbackStatus), feedback) } };
            if (message != "")
                _params.Add("msg", message);
            return sendRequest("/api/feedback/" + username + "/", _params, "post");
        }

        /// <summary>
        /// Посылает сумма Bitcoins из бумажника токена владельца.
        /// Обратите внимание, что этот API требует своего разрешения API под названием Money.
        /// В случае успеха, этот API возвращает только сообщение, указывающее на успех.
        /// Настоятельно рекомендуется, чтобы свести к минимуму срок службы маркеров доступа с разрешением Money.
        /// Вызов /api/logout/ мгновенно удаляет токен
        /// Если указать pin, то будет вызван соответсвующий метод
        /// </summary>
        /// <param name="ammount">Сумма перевода Bitcoins</param>
        /// <param name="address">Адрес Bitcoin</param>
        /// <returns></returns>
        protected string walletSend_as_string(string ammount, string address, string pin = "")
        {
            Dictionary<string, string> _params = new Dictionary<string, string>() { { "ammount", ammount }, { "address", address } };
            if (pin != "")
                _params.Add("pincode", pin);
            return sendRequest("/api/wallet-send/" + (_params.Count == 2 ? "" : "-pin"), _params, "post");
        }

        /// <summary>
        /// Немедленно удаляет текущий API маркер доступа. Чтобы получить новый маркер после этого, общественные приложения должны будут проходить повторную аутентификацию, конфиденциальные приложения могут превратить в знак обновления.
        /// </summary>
        /// <returns></returns>
        protected string Logout_as_string()
        {
            return sendRequest("/api/logout/", null, "post");
        }

        /// <summary>
        /// Изменить/Добавить объявление. Если параметр ad_id указан, то редактирование. В противном случае создаётся новый
        /// </summary>
        /// <param name="price_equation"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="city"></param>
        /// <param name="location_string"></param>
        /// <param name="countrycode"></param>
        /// <param name="currency"></param>
        /// <param name="account_info"></param>
        /// <param name="bank_name"></param>
        /// <param name="msg"></param>
        /// <param name="sms_verification_required"></param>
        /// <param name="track_max_amount"></param>
        /// <param name="require_trusted_by_advertiser"></param>
        /// <param name="require_identification"></param>
        /// <param name="min_amount"></param>
        /// <param name="max_amount"></param>
        /// <param name="opening_hours"></param>
        /// <param name="limit_to_fiat_amounts"></param>
        /// <param name="require_trade_volume"></param>
        /// <param name="require_feedback_score"></param>
        /// <param name="first_time_limit_btc"></param>
        /// <param name="volume_coefficient_btc"></param>
        /// <param name="reference_type"></param>
        /// <param name="display_reference"></param>
        /// <param name="payment_window_minutes"></param>
        /// <param name="floating"></param>
        /// <param name="trade_type"></param>
        /// <param name="online_provider"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        protected string editAd_as_string(string price_equation, string lat, string lon, string city, string location_string, string countrycode, string currency, string account_info, string bank_name, string msg, string sms_verification_required, string track_max_amount, string require_trusted_by_advertiser, string require_identification,
            string ad_id, string min_amount = "", string max_amount = "", string opening_hours = "", string limit_to_fiat_amounts = "",
            /*Опционально для ONLINE_SELL ads:*/string require_trade_volume = "", string require_feedback_score = "", string first_time_limit_btc = "", string volume_coefficient_btc = "", string reference_type = "", string display_reference = "", string details_phone_number = "",
            /*Опционально для ONLINE_BUY ads:*/string payment_window_minutes = "",
            /*Опционально для LOCAL_SELL ads:*/string floating = "",
            /*Только при создании:*/LB_TradeTypes trade_type = LB_TradeTypes.ALL, string online_provider = "",
            /*Только при редактировании:*/string visible = "")
        {
            if (ad_id == "" && trade_type == LB_TradeTypes.ALL)
                return "Для создания объявления - укажите конкретный тип торгов";
            Dictionary<string, string> _params = new Dictionary<string, string>() { { "price_equation", price_equation }, { "lat", lat }, { "lon", lon }, { "city", city }, { "location_string", location_string }, { "countrycode", countrycode }, { "currency", currency }, { "account_info", account_info }, { "bank_name", bank_name }, { "msg", msg }, { "sms_verification_required", sms_verification_required }, { "track_max_amount", track_max_amount }, { "require_trusted_by_advertiser", require_trusted_by_advertiser }, { "require_identification", require_identification } };
            if (min_amount != "") { _params.Add("min_amount", min_amount); }
            if (max_amount != "") { _params.Add("max_amount", max_amount); }
            if (opening_hours != "") { _params.Add("opening_hours", opening_hours); }
            if (limit_to_fiat_amounts != "limit_to_fiat_amounts") { _params.Add("limit_to_fiat_amounts", limit_to_fiat_amounts); }
            if (trade_type == LB_TradeTypes.ONLINE_BUY)
            {
                if (payment_window_minutes != "") { _params.Add("payment_window_minutes", payment_window_minutes); }
            }
            else if (trade_type == LB_TradeTypes.ONLINE_SELL)
            {
                if (require_trade_volume != "") { _params.Add("require_trade_volume", require_trade_volume); }
                if (require_feedback_score != "") { _params.Add("require_feedback_score", require_feedback_score); }
                if (first_time_limit_btc != "") { _params.Add("first_time_limit_btc", first_time_limit_btc); }
                if (volume_coefficient_btc != "") { _params.Add("volume_coefficient_btc", volume_coefficient_btc); }
                if (reference_type != "") { _params.Add("reference_type", reference_type); }
                if (display_reference != "") { _params.Add("display_reference", display_reference); }
                if (details_phone_number != "") { _params.Add("details-phone_number", details_phone_number); }

            }
            else if (trade_type == LB_TradeTypes.LOCAL_SELL)
                if (floating != "") { _params.Add("floating", floating); }

            if (ad_id != "")
                _params.Add("visible", visible);
            else if (ad_id == "" && online_provider != "")
            {
                _params.Add("online_provider", online_provider);
                _params.Add("trade_type", Enum.GetName(typeof(LB_TradeTypes), trade_type));
            }
            //

            if (ad_id == "")
                return sendRequest("/api/ad-create/", _params, "post");// Creation
            else
                return sendRequest("/api/ad/" + ad_id + "/", _params, "post");// Editing
        }
        #endregion
    }
}
