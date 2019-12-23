////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using LocalBitcoinsAPI.Classes.lb_Serialize;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LocalBitcoinsAPI
{
    public class LocalBitcoins_API
    {
        public enum DashboardStates { Open, Released, Canceled, Closed };
        public enum DashboardFilter { Buyer, Seller, All };
        public enum FeedbackStatus { trust, positive, neutral, block, block_without_feedback };
        public enum TransactionTypes { Send = 1, Pending, Other, Fees, Internal };
        public enum TradeType { LOCAL_SELL, LOCAL_BUY, ONLINE_SELL, ONLINE_BUY };
        public enum TradeVisibleStatus { No, Yes };
        public enum LocalAdType { buy, sell };
        //
        public raw_api_LocalBitcoins api_raw;
        //
        private string hmac_auth_key;
        private string hmac_auth_secret;

        public LocalBitcoins_API(string auth_key, string auth_secret)
        {
            hmac_auth_key = auth_key;
            hmac_auth_secret = auth_secret;
            api_raw = new raw_api_LocalBitcoins(auth_key, auth_secret);
        }
        ///////////////////////////////////////////
        #region Advertisements - Announcements of sale/purchase of bitcoins
        #region authenticated (to perform these requests requires authorization on the site)
        /// <summary>
        /// Возвращает все рекламные объявления владельца ключа в виде ad_list данных. Если объявлений много, ответ будет разбит на страницы. Вы можете отфильтровать ответ, используя необязательные аргументы.
        /// </summary>
        /// <returns></returns>
        public AdListBitcoinsOnlineSerializationClass AdsByFilter(bool? visible = null, string trade_type = "", string currency = "", string countrycode = "")
        {
            return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Get_ADs_by_filter(visible, trade_type, currency, countrycode));
        }
        #endregion
        #region public (these requests are public. No authorization required)
        
        /// <summary>
        /// Получить методы оплаты. Все или в определённой стране
        /// </summary>
        /// <param name="countrycode">Код старны (напрмиер RU). Если не указать то возвращается все</param>
        /// <returns></returns>
        public Dictionary<string, PaymentMethodsSerializationClass> PaymentMethods(string countrycode = "")
        {
            string json;
            if (string.IsNullOrEmpty(countrycode))
                json = api_raw.Payment_methods;
            else
                json = api_raw.Payment_methods_by_countrycode(countrycode);

            if (json == null)
                json = string.Empty;

            Regex re = new Regex(@"^{\s*""\s*data\s*""\s*:\s*{\s*""\s*methods\s*""\s*:\s*{\s*(.+)\s*}\s*,\s*""\s*method_count\s*""\s*:\s*\d+\s*}\s*}$");
            MatchCollection m = re.Matches(json);

            if (m.Count == 0 || m[0].Groups.Count < 2)
                return null;

            json = m[0].Groups[1].Value.Trim();
            re = new Regex(@"(""\s*[^""]+\s*""\s*:\s*{\s*[^}]+\s*})", RegexOptions.Compiled);
            m = re.Matches(json);
            re = new Regex(@"""([^""]+)"":(.+)", RegexOptions.Compiled);
            Dictionary<string, PaymentMethodsSerializationClass> pay_metgods = new Dictionary<string, PaymentMethodsSerializationClass>();
            foreach (Match _m in m)
            {
                json = _m.Groups[1].Value.Trim();
                Match my_match = re.Match(json);
                pay_metgods.Add(my_match.Groups[1].Value, (PaymentMethodsSerializationClass)SerializationRoot.ReadObject(typeof(PaymentMethodsSerializationClass), my_match.Groups[2].Value.Trim()));
            }

            return pay_metgods.ToList().OrderBy(x => x.Value.code).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Получить коды стран
        /// </summary>
        public CountryCodesSerializationClass CountryCodes
        {
            get
            {
                return (CountryCodesSerializationClass)SerializationRoot.ReadObject(typeof(CountryCodesSerializationClass), api_raw.CountryCodes);
            }
        }

        /// <summary>
        /// Получить названия и коды валют
        /// </summary>
        public List<ClassCurrencies> Currencies
        {
            get
            {
                string json = api_raw.Currencies.Trim();
                Regex re = new Regex(@"^{\s*""\s*data\s*""\s*:\s*{\s*""\s*currencies\s*""\s*:\s*{\s*(.+)\s*}\s*,\s*""\s*currency_count\s*""\s*:\s*\d+\s*}\s*}$");
                MatchCollection m = re.Matches(json);
                json = m[0].Groups[1].Value.Trim();
                re = new Regex(@"""\s*([^""]+)""\s*:\s*{\s*""\s*name\s*""\s*:\s*""\s*([^""]+)""\s*,\s*""\s*[^""]+""\s*:\s*([^}]+)}");
                m = re.Matches(json);
                List<ClassCurrencies> curr_Currencies = new List<ClassCurrencies>();
                foreach (Match _m in m)
                {
                    string is_altcoin_as_string = _m.Groups[3].Value;
                    bool? is_altcoin = null;
                    switch (is_altcoin_as_string)
                    {
                        case "true":
                            is_altcoin = true;
                            break;
                        case "false":
                            is_altcoin = false;
                            break;
                        default:
                            is_altcoin = null;
                            break;
                    }
                    curr_Currencies.Add(new ClassCurrencies() { code = _m.Groups[1].Value, name = _m.Groups[2].Value, altcoin = is_altcoin });
                }
                return curr_Currencies;
            }
        }

        /// <summary>
        /// Получить места покупки/продажи около GPS lat/lon. 
        /// </summary>
        /// <param name="lat">Широта (например 55.7542359)</param>
        /// <param name="lon">Долгота (например 37.6194498)</param>
        /// <param name="countrycode">Код страны (напрмиер RU)</param>
        /// <param name="location_string">Описание места</param>
        /// <returns></returns>
        public PlacesSerializationClass Places(string lat, string lon, string countrycode = "", string location_string = "")
        {
            return (PlacesSerializationClass)SerializationRoot.ReadObject(typeof(PlacesSerializationClass), api_raw.Places(lat, lon, countrycode, location_string));
        }
        #endregion
        #endregion
        ///////////////////////////////////////////
        #region Trades - transaction manipulation
        #region authenticated (to perform these requests requires authorization on the site)

        #endregion
        #endregion
        ///////////////////////////////////////////
        #region Account
        #region public

        #endregion
        #region authenticated

        #endregion
        #endregion
        ///////////////////////////////////////////
        #region Wallet
        #region public

        #endregion
        #region authenticated

        #endregion
        #endregion
        ///////////////////////////////////////////
        #region Invoices
        #region authenticated

        #endregion
        #endregion
        ///////////////////////////////////////////
        #region Public Market Data
        
        /// <summary>
        /// Получить предложения покупки биткоинов. Либо без фильтра (страна, валюта, метод оплаты), либо с одним из наборов филтров
        /// !!! - Если набор фильтров будет не верным-то запрос вернёт null
        /// ??? - Параметр pagination_url отключает все фильтры. pagination_url используется для разбивки на порции результат и в себе уже имеет все фильтры
        /// 0) Без филтра совсем.
        /// 1) По методу оплаты (payment_method)
        /// 2) По валюте (currency)
        /// 3) По валюте и методу оплаты (currency & payment_method)
        /// 4) По коду старны и названию страны (countrycode & country_name)
        /// 5) По коду старны, названию страны и методу оплаты (countrycode & country_name & payment_method)
        /// </summary>
        /// <param name="countrycode">Код старны</param>
        /// <param name="country_name">Название старны</param>
        /// <param name="currency">Валюта</param>
        /// <param name="payment_method">Метод оплаты</param>
        /// <param name="pagination_url">URL запроса разделения результата на порции/страницы</param>
        public AdListBitcoinsOnlineSerializationClass BuyBitcoinsOnline(string countrycode = "", string country_name = "", string currency = "", string payment_method = "", string pagination_url = "")
        {
            if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty(countrycode) && !string.IsNullOrEmpty(country_name) && !string.IsNullOrEmpty(payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Buy_bitcoins_online_by_country_and_payment(countrycode, country_name, payment_method));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(currency + payment_method) && !string.IsNullOrEmpty(countrycode) && !string.IsNullOrEmpty(country_name))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Buy_bitcoins_online_by_country(countrycode, country_name));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(countrycode + country_name) && !string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty(payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Buy_bitcoins_online_by_currency_and_payment(currency, payment_method));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(countrycode + country_name + payment_method) && !string.IsNullOrEmpty(currency))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Buy_bitcoins_online_by_currency(currency));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(countrycode + country_name + currency) && !string.IsNullOrEmpty(payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Buy_bitcoins_online_by_payment(payment_method));
            }
            else if (!string.IsNullOrEmpty(pagination_url) || string.IsNullOrEmpty(countrycode + country_name + currency + payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Buy_bitcoins_online(pagination_url));
            }
            return null;
        }

        /// <summary>
        /// Получить предложения продажи биткоинов. Либо без фильтра (страна, валюта, метод оплаты), либо с одним из наборов филтров
        /// !!! - Необходимо указывать валюту или страну. Если не указать явно, то определиться по IP пдресу страна и валюта.
        /// !!! - Если набор фильтров будет не верным-то запрос вернёт null
        /// ??? - Параметр pagination_url отключает все фильтры. pagination_url используется для разбивки на порции результат и в себе уже имеет все фильтры
        /// 0) Без филтра совсем.
        /// 1) По методу оплаты (payment_method)
        /// 2) По валюте (currency)
        /// 3) По валюте и методу оплаты (currency & payment_method)
        /// 4) По коду старны и названию страны (countrycode & country_name)
        /// 5) По коду старны, названию страны и методу оплаты (countrycode & country_name & payment_method)
        /// </summary>
        /// <param name="countrycode">Код старны</param>
        /// <param name="country_name">Название старны</param>
        /// <param name="currency">Валюта</param>
        /// <param name="payment_method">Метод оплаты</param>
        /// <param name="pagination_url">URL запроса разделения результата на порции/страницы</param>
        public AdListBitcoinsOnlineSerializationClass SellBitcoinsOnline(string countrycode = "", string country_name = "", string currency = "", string payment_method = "", string pagination_url = "")
        {
            if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty(countrycode) && !string.IsNullOrEmpty(country_name) && !string.IsNullOrEmpty(payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Sell_bitcoins_online_by_country_and_payment(countrycode, country_name, payment_method));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(currency + payment_method) && !string.IsNullOrEmpty(countrycode) && !string.IsNullOrEmpty(country_name))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Sell_bitcoins_online_by_country(countrycode, country_name));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(countrycode + country_name) && !string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty(payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Sell_bitcoins_online_by_currency_and_payment(currency, payment_method));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(countrycode + country_name + payment_method) && !string.IsNullOrEmpty(currency))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Sell_bitcoins_online_by_currency(currency));
            }
            else if (string.IsNullOrEmpty(pagination_url) && string.IsNullOrEmpty(countrycode + country_name + currency) && !string.IsNullOrEmpty(payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Sell_bitcoins_online_by_payment(payment_method));
            }
            else if (!string.IsNullOrEmpty(pagination_url) ||string.IsNullOrEmpty(countrycode + country_name + currency + payment_method))
            {
                return (AdListBitcoinsOnlineSerializationClass)SerializationRoot.ReadObject(typeof(AdListBitcoinsOnlineSerializationClass), api_raw.Sell_bitcoins_online(pagination_url));
            }
            return null;
        }

        /// <summary>
        /// Возвращает список всех завершенных сделок за последние 1-24 часа по всем валютам.
        /// </summary>
        public Dictionary<string, TickerAllCurrenciesSerializationClass> TickerAllCurrencies
        {
            get
            {
                string json = api_raw.Bitcoinaverage_ticker_all_currencies;
                if (!string.IsNullOrEmpty(json))
                    json = json.Trim();

                Regex re = new Regex(@"^{(.+)}$");
                Match m = re.Match(json);
                json = m.Groups[1].Value;
                re = new Regex(@"\s*""\s*([^ ""]+)\s*""\s*:\s*({\s*[^{]+\s*{\s*[^}]+\s*}\s*[^}]*\s*})");
                MatchCollection ms = re.Matches(json);
                Dictionary<string, TickerAllCurrenciesSerializationClass> ret_list = new Dictionary<string, TickerAllCurrenciesSerializationClass>();
                foreach (Match _m in ms)
                {
                    ret_list.Add(_m.Groups[1].Value.Trim(), (TickerAllCurrenciesSerializationClass)SerializationRoot.ReadObject(typeof(TickerAllCurrenciesSerializationClass), _m.Groups[2].Value));
                }
                return ret_list;
            }
        }

        /// <summary>
        /// Все закрытые сделки онлайн покупки/продажи. Обновляются каждые 15 минут. Максимальный размер партии выборки - 500 записей.Используйте "?since=parameter" с последним tid что бы получить нужный сдвиг
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public TradeItemSerializationClass[] Trades(string currency)
        {
            return (TradeItemSerializationClass[])SerializationRoot.ReadObject(typeof(TradeItemSerializationClass[]), api_raw.Bitcoincharts_trades_by_currency(currency));
        }

        /// <summary>
        /// Журнал ордеров
        /// </summary>
        /// <param name="currency">Валюта (например RUB)</param>
        /// <returns></returns>
        public OrdersSerializationClass Orderbook(string currency)
        {
            return (OrdersSerializationClass)SerializationRoot.ReadObject(typeof(OrdersSerializationClass), api_raw.Bitcoincharts_orderbook_by_currency(currency));
        }
        #endregion
    }
}