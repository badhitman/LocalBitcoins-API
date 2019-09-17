////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////

namespace LocalBitcoinsAPI
{
    abstract class abstract_api_LocalBitcoins
    {
        #region Advertisements - Announcements of sale/purchase of bitcoins
        #region authenticated (to perform these requests requires authorization on the site)
        /// <summary>
        /// /api/ads/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns the token owner's all advertisements in the data key ad_list. If there are a lot of ads, the response will be paginated. You can filter the response using the optional arguments.
        /// </summary>
        /// <param name="visible">Optional: [Boolean] 0 for false, 1 for true</param>
        /// <param name="trade_type">Optional: [String] One of LOCAL_SELL, LOCAL_BUY, ONLINE_SELL, ONLINE_BUY</param>
        /// <param name="currency">Optional: [String] Three letter currency code. See list of valid currencies (/api/currencies/)</param>
        /// <param name="countrycode">Optional: [String] Two letter country code. See valid country codes (/api/countrycodes/)</param>
        /// Example response
        ///{
        ///    "data": {
        ///        "visible": boolean,
        ///        "hidden_by_opening_hours": boolean,
        ///        "location_string": human-readable location identifier string,
        ///        "countrycode": countrycode string, two letters,
        ///        "city": city name,
        ///        "trade_type": string, often one of LOCAL_SELL, LOCAL_BUY, ONLINE_SELL, ONLINE_BUY,
        ///        "online_provider": payment method string e.g. NATIONAL_BANK,
        ///        "first_time_limit_btc": string representation of a decimal or null,
        ///        "volume_coefficient_btc": string repr of a decimal,
        ///        "sms_verification_required": boolean
        ///        "reference_type": string, e.g. SHORT,
        ///        "display_reference": boolean,
        ///        "currency": three letter string,
        ///        "lat": float,
        ///        "lon": float,
        ///        "min_amount": string repr of a decimal or null,
        ///        "max_amount": string repr of a decimal or null,
        ///        "max_amount_available": string repr of a decimal or null,
        ///        "limit_to_fiat_amounts": "5,10,20",
        ///        "ad_id": primary key of the ad,
        ///        "temp_price_usd": current price per BTC in USD,
        ///        "floating": boolean if LOCAL_SELL,
        ///        "profile": null or {
        ///            "username": advertisement owner's profile username,
        ///            "name": username, trade count and feedback score combined,
        ///            "last_online": user last seen, ISO formatted date,
        ///            "trade_count": number of trades for user,
        ///            "feedback_score": int
        ///        }
        ///        "require_feedback_score": 50,
        ///        "require_trade_volume": null,
        ///        "require_trusted_by_advertiser": boolean,
        ///        "payment_window_minutes": 30,
        ///        "bank_name": string,
        ///        "track_max_amount": boolean,
        ///        "atm_model": string or null
        ///    },
        ///    "actions": {
        ///        "public_view": URL to view this ad's public HTML page
        ///    }
        ///}
        ///If the advertisement's owner is the same user as the token owner, there are more fields in addition to the above:
        ///{
        ///    "data": {
        ///        ...
        ///        "price_equation": string,
        ///        "opening_hours": "null" or "[[sun_start, sun_end], [mon_start, mon_end], [tue_start, tue_end], [wed_start, wed_end], [thu_start, thu_end], [fri_start, fri_end], [sat_start, sat_end]",
        ///        "account_info": string,
        ///        "account_details": {
        ///            payment method specific fields
        ///        }
        ///    },
        ///    "actions": {
        ///        ...
        ///        "html_edit": URL to view this ad's HTML edit page
        ///                     that has more options than the API change_form does
        ///        "change_form": URL to change this ad
        ///    }
        ///}
        ///If the token owner can create a trade with the advertisement, the contact form URL is also returned.
        ///{
        ///                    ...
        ///                    "actions": { "contact_form": URL to contact form API for this ad}
        ///}
        /// Note that ATM advertisements are also returned by the API. Contacts cannot be created to ATM advertisements if the contact_form action is missing.
        /// For ONLINE_BUY advertisements a is_low_risk boolean is returned. If this is set TRUE, the ad has displays a green thumb on the website.
        /// min_amount and max_amount are in denominated in currency.
        /// track_max_amount is the same as the advertisement option "Track liquidity" on web site.
        abstract protected string ADs_by_filters(bool? visible = null, string trade_type = "", string currency = "", string countrycode = "");

        /// <summary>
        /// /api/ad-get/{ad_id}
        /// Permissions: Read;
        /// HTTP method: GET;
        ///  - Returns information of single advertisement based on the ad ID, it returns the same fields as /api/ads/.
        /// If a valid advertisement ID is provided the API response returns the ad within a list structure.If the advertisement is not viewable an error is returned.
        /// </summary>
        /// <param name="id_ad">Required: [String] ID of advertisement</param>
        abstract protected string Get_AD(string ad_id);

        /// <summary>
        /// /api/ad-get/
        /// Permissions: Read;
        /// HTTP method: GET;
        ///  - Returns all advertisements from a comma-separated list of ad IDs. Invalid advertisement ID's are ignored and no error is returned. Otherwise it functions the same as /api/ad-get/{ad_id}/. A maximum of 50 advertisements are returned at a time. Advertisements are not returned in any particular order.
        /// </summary>
        /// <param name="ids_ads">Required: [String] Comma separated list of advertisement IDs.</param>
        abstract protected string Get_ADs(string ads);

        /// <summary>
        /// /api/ad/{ad_id}/
        /// Permissions: Read, Write;	
        /// HTTP method: POST;
        /// - Sending a request to /api/ad/{ad_id}/ with the HTTP method POST for an advertisement ID that was created by the token owner results in the advertisement being updated.
        /// NOTE: Omitting optional requirements, such as min_amount, max_amount or opening_hours, will unset them. You may want to pass through all fields from the /api/ads/ records you're editing. Fields require_trade_volume, require_feedback_score, first_time_limit_btc, volume_coefficient_btc, payment_window_minutes, reference_type, display_reference and limit_to_fiat_amounts are not cleared if they are omitted.
        /// Errors in the new values are reported in the errors key. If there are any errors, no changes will be made to the ad.
        /// This API endpoint functions the same as /api/ad-create/ apart from the addition of the argument visible that allows you to turn the advertisement visibility on and off. Please see /api/ad-create/ for further information on the arguments for this API endpoint.
        /// Do not make frequent calls to this endpoint. If you want to update price equation often, please use this API call instead (/api/ad-equation/{ad_id}/)
        /// </summary>
        /// <param name="price_equation">Required: [String] Price equation formula</param>
        /// <param name="lat">Required: [Integer] Latitude coordinate</param>
        /// <param name="lon">Required: [Integer] Longitude coordinate</param>
        /// <param name="city">Required: [String] City name</param>
        /// <param name="location_string">Required: [String] Human readable location text.</param>
        /// <param name="countrycode">Required: [String] Two-character country code. See valid country codes (/api/countrycodes/)</param>
        /// <param name="currency">Required: [String] Three letter currency code. See list of valid currencies (/api/currencies/). With ONLINE_SELL and ONLINE_BUY ads, some payment methods might limit the allowed currencies (/api/payment_methods/). Altcoin currencies are only allowed with ONLINE_SELL and ONLINE_BUY ads, if they are included in the currency list of payment method.</param>
        /// <param name="account_info">Required: [String] - </param>
        /// <param name="bank_name">Required: [String] Certain of the online payment methods require bank_name to be chosen from a limited set of names. To find out these limited choices, use this public API request /api/payment_methods/</param>
        /// <param name="msg">Required: [String] Terms of trade of the advertisement</param>
        /// <param name="sms_verification_required">Required: [Boolean] 0 for false, 1 for true.</param>
        /// <param name="track_max_amount">Required: [Boolean] 0 for false, 1 for true.</param>
        /// <param name="require_trusted_by_advertiser">Required: [Boolean] 0 for false, 1 for true.</param>
        /// <param name="require_identification">Required: [Boolean] 0 for false, 1 for true.</param>
        /// <param name="min_amount">Optional: [Integer] Minimum transaction limit in fiat.</param>
        /// <param name="max_amount">Optional: [Integer] Maximum transaction limit in fiat.</param>
        /// <param name="opening_hours">Optional: [JSON array] Times when ad is visible (Set according to timezone set with token owners account)</param>
        /// <param name="limit_to_fiat_amounts">Optional: [String] Comma separated fiat value list of amounts to restrict. Same as "Restrict amounts to" on site</param>
        /// <param name="visible">Optional: [Boolean] 0 for false, 1 for true.</param>
        /// <param name="require_trade_volume">'Optional arguments ONLINE_SELL ads': [Integer] - </param>
        /// <param name="require_feedback_score">'Optional arguments ONLINE_SELL ads': [Integer] - </param>
        /// <param name="first_time_limit_btc">'Optional arguments ONLINE_SELL ads': [Integer] - </param>
        /// <param name="volume_coefficient_btc">'Optional arguments ONLINE_SELL ads': [Integer] - </param>
        /// <param name="reference_type">'Optional arguments ONLINE_SELL ads': [String] Supported values are SHORT, LONG, NUMERIC, LETTERS</param>
        /// <param name="display_reference">'Optional arguments ONLINE_SELL ads': [Boolean] Show reference in trades opened using this ad. 0 for false, 1 for true.</param>
        /// <param name="payment_window_minutes">'Optional arguments ONLINE_BUY': [Integer] Payment window time in minutes.</param>
        /// <param name="floating">'Optional arguments LOCAL_SELL': [Boolean] Enable floating price for the ad. 0 for false, 1 for true.</param>
        abstract protected string Edit_AD(string ad_id, string price_equation, int lat, int lon, string city, string location_string, string countrycode, string currency, string account_info, string bank_name, string msg, bool sms_verification_required, bool track_max_amount, bool require_trusted_by_advertiser, bool require_identification, int min_amount = -1, int max_amount = -1, string[] opening_hours = null, string limit_to_fiat_amounts = "", bool? visible = null, int require_trade_volume = -1, int require_feedback_score = -1, int first_time_limit_btc = -1, int volume_coefficient_btc = -1, string reference_type = "", bool? display_reference = null, int payment_window_minutes = -1, bool? floating = null);

        /// <summary>
        /// /api/ad-create/
        /// Permissions: Read, Write;	
        /// HTTP method: POST;
        /// - This API endpoint is for creating new advertisements for the token owner. Only fields listed above can be used with the API, use the web site to change the rest of them.
        /// opening_hours is given as JSON array in the same format as it is returned when getting details of ads.Weekdays start from Sunday and each weekday contains start and end that are measured as 15 minutes.So for example 4 means 01:00 and 5 means 01:15.
        /// Some of the online payment methods require bank_name to be chosen from a limited set of names.To find out these limited choices, use this public API request /api/payment_methods/. They can be found from the key bank_name_choices.
        /// ONLINE_SELL advertisements may have (https://localbitcoins.net/api-docs/online-sell-fields/) additional fields you need to provide, depending on the advertisement's payment method (online_provider).
        /// Local Advertisements
        /// Creating Local advertisements functions the same as online advertisements but with a couple of differences:
        /// Local ads have a distance field on each ad, supplied in kilometers.
        /// Local ads are not paginated.You can, however, look up ads from other nearby locations as a workaround.
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
        /// <param name="online_provider"></param>
        /// <param name="trade_type"></param>
        /// <param name="min_amount"></param>
        /// <param name="max_amount"></param>
        /// <param name="opening_hours"></param>
        /// <param name="limit_to_fiat_amounts"></param>
        /// <param name="visible"></param>
        /// <param name="require_trade_volume"></param>
        /// <param name="require_feedback_score"></param>
        /// <param name="first_time_limit_btc"></param>
        /// <param name="volume_coefficient_btc"></param>
        /// <param name="reference_type"></param>
        /// <param name="display_reference"></param>
        /// <param name="payment_window_minutes"></param>
        /// <param name="floating"></param>
        abstract protected string Create_AD(string price_equation, int lat, int lon, string city, string location_string, string countrycode, string currency, string account_info, string bank_name, string msg, bool sms_verification_required, bool track_max_amount, bool require_trusted_by_advertiser, bool require_identification, string online_provider, string trade_type, int min_amount = -1, int max_amount = -1, string[] opening_hours = null, string limit_to_fiat_amounts = "", bool? visible = null, int require_trade_volume = -1, int require_feedback_score = -1, int first_time_limit_btc = -1, int volume_coefficient_btc = -1, string reference_type = "", bool? display_reference = null, int payment_window_minutes = -1, bool? floating = null);

        /// <summary>
        /// /api/ad-equation/{ad_id}/
        /// Permissions: Read, Write;		
        /// HTTP method: POST;
        /// - Update price equation of an advertisement. If there are problems with new equation, the price and equation are not updated and advertisement remains visible.
        /// </summary>
        /// <param name="id_ad">Required: [String] ID of advertisement</param>
        /// <param name="price_equation">Required: [String] Price equation formula</param>
        abstract protected string Equation_AD(string id_ad, string price_equation);

        /// <summary>
        /// /api/ad-delete/{ad_id}/
        /// Permissions	Read, Write;
        /// HTTP method POS;
        /// - Sending a request to this endpoint with an advertisement ID created by the token owner deletes the advertisement
        /// </summary>
        /// <param name="id_ad">Required: [String] ID of advertisement</param>
        abstract protected string Delete_AD(string id_ad);
        #endregion
        #region public (these requests are public. No authorization required)
        /// <summary>
        /// /api/payment_methods/
        /// HTTP method: GET;
        /// - Returns a list of valid payment methods. Also contains name and code for payment methods, and possible limitations in currencies and bank name choices.
        /// </summary>
        abstract protected string Payment_methods { get; }

        /// <summary>
        /// /api/payment_methods/{countrycode}/
        /// HTTP method: GET;
        /// - Returns a list of valid payment methods filtered by countrycodes (/api/countrycodes/). Also contains name and code for payment methods, and possible limitations in currencies and bank name choices.
        /// </summary>
        /// <param name="countrycode"></param>
        /// <returns></returns>
        abstract protected string Payment_methods_by_countrycode(string countrycode);

        /// <summary>
        /// /api/countrycodes/
        /// HTTP method: GET;
        /// - List of valid and recognized countrycodes for LocalBitcoins.com. Return value is structured like so:
        /// </summary>
        abstract protected string Countrycodes { get; }

        /// <summary>
        /// /api/currencies/
        /// HTTP method: GET;
        /// - List of valid and recognized fiat currencies for LocalBitcoins.com. Also contains human readable name for every currency and boolean that tells if currency is an altcoin.
        /// </summary>
        abstract protected string Currencies { get; }

        /// <summary>
        /// /api/places/
        /// HTTP method: GET
        /// - Looks up places near lat, lon and provides full URLs to buy and sell listings for each.
        /// You can use external services like Google Places to find lat/lon coordinates for addresses and the like.That way you can also get countrycode and location_string values to improve your lookup.
        /// An InvalidParameter (error code 11) is returned if the parameters did not seem valid.
        /// </summary>
        /// <param name="lat">Required: [Integer] Latitude coordinate</param>
        /// <param name="lon">Required: [Integer] Longitude coordinate</param>
        /// <param name="countrycode">Optional: [String] Two-character country code. See valid country codes (/api/countrycodes/)</param>
        /// <param name="location_string">Optional: [String] Human readable location text.</param>
        /// <returns></returns>
        abstract protected string Places(int lat, int lon, string countrycode = "", string location_string = "");
        #endregion
        #endregion

        #region Trades - transaction manipulation
        #region authenticated (to perform these requests requires authorization on the site)
        /// <summary>
        /// /api/feedback/{username}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Gives feedback to user. Possible feedback values are: trust, positive, neutral, block, block_without_feedback.
        /// This is only possible to set if there is a trade between the token owner and the user specified in {username}
        /// that is canceled or released.
        /// You may also set feedback message using msg field with few exceptions.Feedback block_without_feedback clears the message and with block the message is 
        /// </summary>
        /// <param name="username">Required: [String] username for feedback</param>
        /// <param name="feedback">Required: [String] Allowed values are: trust, positive, neutral, block, block_without_feedback.</param>
        /// <param name="msg">Optional: [String] Feedback message displayed alongside feedback on receivers profile page.</param>
        abstract protected string Feedback_to_user(string username, string feedback, string msg = "");
        
        /// <summary>
        /// /api/contact_release/{contact_id}/
        /// Permissions: Money;
        /// HTTP method: POST;
        /// - Releases Bitcoin trades specified by ID {contact_id}. If the release was successful a message is returned on the data key.
        /// </summary>
        /// <param name="contact_id">Required: [Integer] contact ID</param>
        abstract protected string Release_contact(string contact_id);
        
        /// <summary>
        /// /api/contact_release_pin/{contact_id}/
        /// Permissions: Money_pin;
        /// HTTP method: POST;
        /// - Releases Bitcoin trades specified by ID {contact_id}. if the current pincode is provided. If the release was successful a message is returned on the data key.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        /// <param name="pincode">Required: [Integer] User Apps PIN code</param>
        abstract protected string Release_contact_of_pin(string contact_id, int pincode);
        
        /// <summary>
        /// /api/contact_mark_as_paid/{contact_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Marks a trade as paid.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        abstract protected string Mark_as_paid_contact(string contact_id);
        
        /// <summary>
        /// /api/contact_messages/{contact_id}/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns all chat messages from the trade. Messages are on the message_list key. The structure of a message is as follows:
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        /// Example response
        ///{
        ///    "msg": "message body",
        ///    "sender": {
        ///        "id": 123,
        ///        "name": "bitcoinbaron (0)",
        ///        "username": "bitcoinbaron",
        ///        "trade_count": 0,
        ///        "last_online": "2013-12-17T03:31:12.382862+00:00"
        ///    },
        ///    "created_at": "2013-12-19T16:03:38.218039",
        ///    "is_admin": false,
        ///    "attachment_name": "cnXvo5sV6eH4-some_image.jpg",
        ///    "attachment_type": "image/jpeg",
        ///    "attachment_url": "https://localbitcoins.com/api/..."
        ///}
        abstract protected string Get_messages_from_contact(string contact_id);
        
        /// <summary>
        /// /api/contact_message_post/{contact_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Posts a message and/or uploads an image to the trade. Encode images with multipart/form-data encoding.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        /// <param name="msg">Required (one or both with 'document'): [String] Chat message to trade chat.</param>
        /// <param name="document">Required (one or both with 'msg'): [bytes/file] Image attachments encoded with multipart/form-data</param>
        abstract protected string Post_message_to_contact(string contact_id, string msg, byte[] document);
        
        /// <summary>
        /// /api/contact_dispute/{contact_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Starts a dispute on the specified trade ID if the requirements for starting the dispute has been fulfilled. See the FAQ for up-to-date details regarding dispute requirements.
        /// You can provide a short (optional) description using topic. This helps customer support to deal with the problem.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        /// <param name="topic">Optional: [String] Short description of issue to LocalBitcoins customer support.</param>
        abstract protected string Dispute_contact(string contact_id, string topic = "");
        
        /// <summary>
        /// /api/contact_cancel/{contact_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Cancels the trade if the token owner is the Bitcoin buyer. Bitcoin sellers cannot cancel trades.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        abstract protected string Cancel_contact(string contact_id);
        
        /// <summary>
        /// /api/contact_fund/{contact_id}
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Attempts to fund an unfunded local trade from the token owners wallet. Works only if the token owner is the Bitcoin seller in the trade.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        abstract protected string Fund_contact(string contact_id);
        
        /// <summary>
        /// /api/contact_mark_realname/{contact_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Creates or updates real name confirmation.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        /// <param name="confirmation_status">Required: [Integer] 1 = Name matches | 2 = Name was different | 3 = Name was not checked | 4 = Name was not visible</param>
        /// <param name="id_confirmed">Required: [Boolean] 0 for false, 1 for true.</param>
        abstract protected string Mark_realname_contact(string contact_id, int confirmation_status, bool id_confirmed);
        
        /// <summary>
        /// /api/contact_mark_identified/{contact_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Marks the identity of trade partner as verified. You must be the advertiser in this trade.
        /// </summary>
        /// <param name="contact_id">Required: [String] contact ID</param>
        abstract protected string Mark_identified_contact(string contact_id);
        
        /// <summary>
        /// /api/contact_create/{ad_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Attempts to start a Bitcoin trade from the specified advertisement ID.
        /// Amount is a number in the advertisement's fiat currency.
        /// Returns the API URL to the newly created contact at actions.contact_url.Whether the contact was able to be funded automatically is indicated at data.funded.Only non-floating LOCAL_SELL may return unfunded, all other trade types either fund or fail.
        /// ONLINE_BUY advertisements may have additional fields you need to provide depending on the advertisement's payment method (online_provider).
        /// </summary>
        /// <param name="ad_id">Required: [String] AD ID</param>
        /// <param name="amount">Required: [Integer] Number in the advertisement's fiat currency.</param>
        /// <param name="message">Optional: [String] Optional message to send to the advertiser.</param>
        abstract protected string Create_contact(string ad_id, int amount, string message = "");
        
        /// <summary>
        /// /api/contact_info/{contact_id}/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns information about a single trade that the token owner is part in.
        /// </summary>
        /// <param name="ad_id">Required: [String] AD ID</param>
        abstract protected string Contact_info(string ad_id);
        
        /// <summary>
        /// /api/contact_info/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - contacts is a comma-separated list of contact IDs that you want to access in bulk. The token owner needs to be either a buyer or seller in the contacts, contacts that do not pass this check are simply not returned.
        /// A maximum of 50 contacts can be requested at a time.
        /// The contacts are not returned in any particular order.
        /// </summary>
        /// <param name="contacts">Required: [String] CSV list of trade ID numbers.</param>
        abstract protected string Contacts_info(string contacts);
        #endregion
        #endregion

        #region Account
        #region public
        /// <summary>
        /// /api/account_info/{username}/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - This API request lets you retrieve the public user information on a LocalBitcoins user. The response contains the same information that is found on an account's public profile page.
        /// Making this request with authentication returns extra information.
        /// If you have left feedback to the account you've requested the field my_feedback will have one of the following string values: trust, positive, neutral, block, block_without_feedback.
        /// If you have also set a feedback message for the user, it can be found from field my_feedback_msg.
        /// </summary>
        /// <param name="username">Required: [String] Username</param>
        /// Example output
        ///{
        ///  "data": {
        ///     "username": "bitcoinbaron",
        ///     "created_at": "2013-06-25T14:11:30+00:00",
        ///     "trading_partners_count": 0,
        ///     "feedbacks_unconfirmed_count": 0,
        ///     "trade_volume_text": "Less than 25 BTC",
        ///     "has_common_trades": false,
        ///     "confirmed_trade_count_text": "0",
        ///     "blocked_count": 0,
        ///     "feedback_score": 0,
        ///     "feedback_count": 0,
        ///     "url": "https://localbitcoins.com/p/bitcoinbaron/",
        ///     "trusted_count": 0,
        ///     "identity_verified_at": null or date,
        ///     "real_name_verifications_trusted": 1,
        ///     "real_name_verifications_untrusted": 4,
        ///     "real_name_verifications_rejected": 0
        ///   }
        ///}
        abstract protected string Account_info(string username);
        #endregion
        #region authenticated
        /// <summary>
        /// /api/dashboard/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns a list of trades on the data key contact_list. This API end point mirrors the website's dashboard, allowing access to contacts in different states.
        /// In addition all of these listings have buyer/ and seller/ sub-listings to view contacts where the token owner is either buying or selling, respectively. E.g. /api/dashboard/buyer/. All contacts where the token owner is participating are returned.
        /// Structure of common values to all contacts is follows:
        ///{
        ///  "data": {
        ///    "created_at": "2013-12-06T15:23:01.61",
        ///    "buyer": {
        ///        "username": "hylje",
        ///        "trade_count": "30+",
        ///        "feedback_score": "100",
        ///        "name": "hylje (30+; 100)",
        ///        "last_online": "2013-12-19T08:28:16+00:00",
        ///        "real_name": string or null if ONLINE trade where you are the seller,
        ///        "company_name": string or null if ONLINE trade where you are the seller,
        ///        "countrycode_by_ip": string or null if ONLINE trade where you are the seller,
        ///        "countrycode_by_phone_number": string or null if ONLINE trade where you are the seller
        ///    }
        ///    "seller": {
        ///        "username": "jeremias",
        ///        "trade_count": "100+",
        ///        "feedback_score": "100",
        ///        "name": "jeremias (100+; 100)",
        ///        "last_online": "2013-12-19T06:28:51+00:00"
        ///    }
        ///    "reference_code": "123",
        ///    "currency": "EUR",
        ///    "amount": "105.55",
        ///    "amount_btc": "190",
        ///    "fee_btc": "1.9",
        ///    "exchange_rate_updated_at": "2013-06-20T15:23:01+00:00",
        ///    "advertisement": {
        ///       "id": 123,
        ///       "trade_type": "ONLINE_SELL"
        ///       "advertiser": {
        ///           "username": "jeremias",
        ///           "trade_count": "100+",
        ///           "feedback_score": "100",
        ///           "name": "jeremias (100+; 100)",
        ///           "last_online": "2013-12-19T06:28:51.604754+00:00"
        ///       }
        ///    },
        ///    "contact_id": 1234
        ///    "canceled_at": null,
        ///    "escrowed_at": "2013-12-06T15:23:01+00:00",
        ///    "funded_at": "2013-12-06T15:23:01+00:00",
        ///    "payment_completed_at": "2013-12-06T15:23:01+00:00",
        ///    "disputed_at": null,
        ///    "closed_at": null,
        ///    "released_at": null,
        ///    "is_buying": true,
        ///    "is_selling": false,
        ///    "account_details": ! see below,
        ///    "account_info": Payment details of ONLINE_SELL as string, if account_details is missing.,
        ///    "floating": boolean if LOCAL_SELL
        ///  },
        ///  "actions": {
        ///    "mark_as_paid_url": "/api/contact_mark_as_paid/1/",
        ///    "advertisement_public_view": "/ads/123",
        ///    "message_url": "/api/contact_messages/1234",
        ///    "message_post_url": "/api/contact_message_post/1234"
        ///  }
        ///}
        /// </summary>
        abstract protected string Dashboard { get; }
        
        /// <summary>
        /// /api/dashboard/released/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns a list of all released trades where the token owner is either a buyer or seller. See /api/dashboard/ for more information.
        /// </summary>
        abstract protected string Dashboard_released { get; }
        
        /// <summary>
        /// /api/dashboard/canceled/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns a list of all canceled trades where the token owner is either a buyer or seller. See /api/dashboard/ for more information.
        /// </summary>
        abstract protected string Dashboard_canceled { get; }
        
        /// <summary>
        /// /api/dashboard/closed/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns a list of all closed trades where the token owner is either a buyer or seller. See /api/dashboard/ for more information.
        /// </summary>
        abstract protected string Dashboard_closed { get; }

        /// <summary>
        /// /api/logout/
        /// Permissions: Read;
        /// HTTP method: POST;
        /// - Expires the current access token immediately. To get a new token afterwards, public apps will need to re-authenticate, confidential apps can turn in a refresh token.
        /// </summary>
        /// <returns></returns>
        abstract protected string Logout();

        /// <summary>
        /// /api/myself/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns information of the currently logged in user (the owner of authentication token). The format is the same as in /api/account_info/{username}.
        ///{
        ///  "data": {
        ///     "username": "bitcoinbaron",
        ///     "created_at": "2013-06-25T14:11:30+00:00",
        ///     "trading_partners_count": 0,
        ///     "feedbacks_unconfirmed_count": 0,
        ///     "trade_volume_text": "Less than 25 BTC",
        ///     "has_common_trades": false,
        ///     "confirmed_trade_count_text": "0",
        ///     "blocked_count": 0,
        ///     "feedback_score": 0,
        ///     "feedback_count": 0,
        ///     "url": "https://localbitcoins.com/p/bitcoinbaron/",
        ///     "trusted_count": 0,
        ///     "identity_verified_at": null or date,
        ///     "real_name_verifications_trusted": 1,
        ///     "real_name_verifications_untrusted": 4,
        ///     "real_name_verifications_rejected": 0
        ///   }
        ///}
        /// </summary>
        abstract protected string Myself { get; }

        /// <summary>
        /// /api/notifications/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns recent notifications. Example result can be seen below:
        ///{
        ///    "data": [
        ///        {
        ///            "url": "/request/online_sell_seller/7126534",
        ///            "created_at": "2016-10-19T13:42:42+00:00",
        ///            "contact_id": 7126534,
        ///            "read": false,
        ///            "msg": "You have a new offer #7126534!",
        ///            "id": "fc431e381909"
        ///        },
        ///        {
        ///            "url": "/ads_edit/323473",
        ///            "created_at": "2016-10-19T13:42:42+00:00",
        ///            "advertisement_id": 323473,
        ///            "read": false,
        ///            "msg": "Your ad has been hidden because Track Liquidity reached the minimum amount. Change the setting or increase the ad's max amount.",
        ///            "id": "469912a4e83a"
        ///        }
        ///    ]
        ///}
        /// </summary>
        abstract protected string Notifications { get; }

        /// <summary>
        /// /api/notifications/mark_as_read/{notification_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Marks a specific notification as read.
        /// </summary>
        /// <param name="id_notification">Required: [String] ID notification</param>
        abstract protected string Mark_as_read_notification(string id_notification);

        /// <summary>
        /// /api/pincode/
        /// Permissions: Read;
        /// HTTP method: POST;
        /// - Checks the given PIN code against the token owners currently active PIN code. You can use this method to ensure the person using the session is the legitimate user.
        /// Due to only requiring the read scope, the user is not guaranteed to have set a PIN code.If you protect your application using this request, please make the user has set a PIN code for his account.
        /// </summary>
        /// <param name="pincode">Required: [Integer] 4 digit app PIN code set from profile settings</param>
        abstract protected string Pincode(int pincode);

        /// <summary>
        /// /api/real_name_verifiers/{username}/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns list of real name verifiers for the user. Returns a list only when you have a trade with the user where you are the seller.
        /// [{"username": "henu.tester", "verified_at": "2016-10-13T13:49:45+00:00"}]
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        abstract protected string Real_name_verifiers(string username);

        /// <summary>
        /// /api/recent_messages/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Returns maximum of 25 newest trade messages. Does not return messages older than one month. Messages are ordered by sending time, and the newest one is first. The list has same format as /api/contact_messages/, but each message has also contact_id field.
        /// If you would like to see messages after a specific date, use the parameter after.It takes UTC date in ISO 8601 format.Only messages that are sent after the date are listed. This is useful if you are polling the call repeatedly and want to avoid receiving the same messages again.
        /// ***
        /// Transaction types
        /// Transactions in the Wallet API have several types. Currently the more elaborate types cover sends.All received transactions are of type 3.
        /// 1	Send	to_address: the recipient Bitcoin address
        /// 2	Pending Send.The transaction processor has not yet published a transaction containing this send to the Bitcoin network, but will do so as quickly as possible.to_address: the to-be-recipient Bitcoin address
        /// 3	Other transactions, e.g.received transactions. See the description field.	—
        /// 4	Bitcoin network fees    —
        /// 5	Internal send to another LocalBitcoins wallet	—
        /// </summary>
        /// <param name="after">Optional: [DateTime] Return messages before date. UTC date in ISO 8601 format</param>
        abstract protected string Recent_messages(System.DateTime after);
        #endregion
        #endregion

        #region Wallet
        #region authenticated
        /// <summary>
        /// /api/wallet/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Gets information about the token owner's wallet balance. The information is as follows:
        ///{
        ///  "message": "OK",
        ///  "total": {
        ///    "balance": "0.05",
        ///    "sendable": "0.05"
        ///  },
        ///  "sent_transactions_30d": [
        ///    {"txid": ...
        ///     "amount": "0.05",
        ///     "description": "Internal send",
        ///     "tx_type": 5,
        ///     "created_at": "2013-12-20T15:27:36+00:00"
        ///  }
        ///  ],
        ///  "received_transactions_30d": [...],
        ///  "receiving_address": "1CsgXvijdeFr47JfATYgV7XzUrrAAsds12",
        ///  "old_address_list": [{
        ///    "address": "15HfUY9Lw...",
        ///    "received": "0.0"
        ///  }]
        ///}
        ///
        /// The transaction lists are for the last 30 days. There is no LocalBitcoins API to fetch older transactions than that. Returns max 10 old addresses. The old addresses are truncated, because they are not meant to be used. If the account is new, the receiving_address might be null. In this case you can create a new address with /api/wallet-addr/.
        /// </summary>
        abstract protected string Wallet { get; }

        /// <summary>
        /// /api/wallet-balance/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Same as /api/wallet/ above, but only returns the message, receiving_address and total fields.
        /// Use this instead if you don't care about transactions at the moment.
        /// </summary>
        abstract protected string Wallet_balance { get; }

        /// <summary>
        /// /api/wallet-send/
        /// Permissions: Money;
        /// HTTP method: POST;
        /// - Sends amount of bitcoins from the token owner's wallet to address.
        /// On success, the response returns a message indicating success.
        /// It is highly recommended to minimize the lifetime of access tokens with the money permission.Request /api/logout/ to make the current token expire instantly.
        /// </summary>
        /// <param name="address">Required: [string] Bitcoin address where you're sending Bitcoin to.</param>
        /// <param name="amount">Required: [Integer] Amount of Bitcoin to send.</param>
        abstract protected string Wallet_send(string address, int amount);

        /// <summary>
        /// /api/wallet-send-pin/
        /// Permissions: Money_PIN;
        /// HTTP method: POST;
        /// </summary>
        /// <param name="address">Required: [string] Bitcoin address where you're sending Bitcoin to.</param>
        /// <param name="amount">Required: [Integer] Amount of Bitcoin to send.</param>
        /// <param name="pincode">Required: [Integer] Token owners PIN code. See /api/pincode/.</param>
        abstract protected string Wallet_send_pin(string address, int amount, int pincode);

        /// <summary>
        /// /api/wallet-addr/
        /// Permissions: Read;
        /// HTTP method: POST;
        /// - Returns an unused receiving address from the token owner's wallet. The address is returned in the address key of the response. Note that this API may keep returning the same (unused) address if requested repeatedly.
        /// </summary>
        abstract protected string Wallet_addr { get; }
        #endregion
        #region public
        /// <summary>
        /// HTTP method: GET;
        /// - This API returns the current outgoing and deposit fees in bitcoins (BTC).
        ///{
        ///"data": {
        ///    "deposit_fee": "0.0005",
        ///    "outgoing_fee": "0.0002"
        ///        }
        ///}
        /// </summary>
        abstract protected string Fees { get; }
        #endregion
        #endregion

        #region Invoices
        #region authenticated
        /// <summary>
        /// /api/merchant/invoices/
        /// Permissions: Read;
        /// HTTP method: GET;
        /// - Lists all invoices. If there are a lot of invoices, the listing will be paginated. Data structure is described below. invoice_list contains objects that are identical to those in API request /api/merchant/invoice/{invoice_id}/.
        /// </summary>
        abstract protected string Invoices { get; }

        /// <summary>
        /// /api/merchant/new_invoice/
        /// Permissions: Read;
        /// HTTP method: POST;
        /// - Creates a new invoice. currency is a three letter currency code, for example USD. Please see a list of all valid currencies (/api/currencies/). For more information how invoices work, check Merchant dashboard (https://localbitcoins.net/merchant/). You can check response structure from API request /api/merchant/invoice/{invoice_id}/
        /// After invoice has been created, you need to deliver its URL to your customer or redirect him/her to it.
        /// Invoices defined as internal can only be paid using a LocalBitcoins account.This is useful for saving Bitcoin transaction fees both for receiver and sender.
        /// You can use return_url to redirect customers automatically back to your site. Redirection happens after the invoice has been paid.
        /// If redirecting fails, the customer can also click the return link manually. Make sure it does not cause problems if the return_url is visited multiple times.
        /// Also note, that return_url is not guaranteed to stay secret from the customer, so do not consider the invoice been paid when the URL is being visited.
        /// Instead use the API request /api/merchant/invoice/{ invoice_id}/ to check payment status.
        /// </summary>
        /// <param name="currency">Required: [String] Three letter currency code. See list of valid currencies (/api/currencies/)</param>
        /// <param name="amount">Required: [Decimal] The amount in the specified currency.</param>
        /// <param name="description">Required: [String] Description new invoice</param>
        /// <param name="lb_internal">Optional: [Boolean] 1 to limit payments to LocalBitcoins accounts, 0 to allow payments from any Bitcoin wallet</param>
        /// <param name="return_url">Optional: [String] URL to automatically redirect customers to after invoice is paid.</param>
        abstract protected string New_invoice(string currency, decimal amount, string description, bool? lb_internal = null, string return_url = "");

        /// <summary>
        /// /api/merchant/invoice/{invoice_id}/
        /// Permissions: READ;
        /// HTTP method: GET;
        /// - Returns information about a specific invoice created by the token owner. Example result can be seen below:
        ///{
        ///    "data": {
        ///        "invoice": {
        ///            "description": "Stuff",
        ///            "created": "2014-09-11T09:34:30.867980",
        ///            "url": "https://localbitcoins.com/merchant/invoice/VD1j3VP7MSjTJjxaptjnFN",
        ///            "amount": "4.99",
        ///            "internal": true,
        ///            "currency": "EUR",
        ///            "state": "NOT_OPENED",
        ///            "id": "VD1j3VP7MSjTJjxaptjnFN",
        ///            "btc_amount": null,
        ///            "btc_address": null,
        ///            "deleting_allowed": true
        ///        }
        ///    }
        ///}
        ///
        /// State is one of these: NOT_OPENED, WAITING_FOR_PAYMENT, PAID, DIDNT_PAID, PAID_IN_LATE, PAID_PARTLY, PAID_AND_CONFIRMED, PAID_IN_LATE_AND_CONFIRMED, PAID_PARTLY_AND_CONFIRMED.
        /// </summary>
        /// <param name="invoice_id">Required: [String] Invoice ID</param>
        abstract protected string Invoice(string invoice_id);

        /// <summary>
        /// /api/merchant/delete_invoice/{invoice_id}/
        /// Permissions: Read, Write;
        /// HTTP method: POST;
        /// - Deletes a specific invoice. Deleting invoices is possible when it is sure that receiver cannot accidentally pay the invoice at the same time as the merchant is deleting it. You can use the API request /api/merchant/invoice/{invoice_id}/ to check if deleting is possible.
        /// </summary>
        /// <param name="invoice_id">Required: [String] Invoice ID</param>
        abstract protected string Delete_invoice(string invoice_id);
        #endregion
        #endregion

        #region Public Market Data
        /// <summary>
        /// /buy-bitcoins-with-cash/{location_id}/{location_slug}/.json
        /// HTTP method: GET;
        /// - This endpoint returns local advertisements. You can give lat and lon - parameters to specify a location near {location_id}, up to 50 kilometers away.
        /// The following API is useful to - the {location_id} and {location_slug} for a lat/lon.
        /// </summary>
        /// <param name="location_id">Required: [String] Location ID</param>
        /// <param name="location_slug">Required: [String] Location slug</param>
        abstract protected string Buy_bitcoins_with_cash(string location_id, string location_slug);

        /// <summary>
        /// /sell-bitcoins-for-cash/{location_id}/{location_slug}/.json
        /// HTTP method: GET;
        /// - This endpoint looks up local ads. You can give lat and lon - parameters to specify a location near {location_id}, up to 50 kilometers away.
        /// The following API is useful to - the {location_id} and {location_slug} for a lat/lon.
        /// </summary>
        /// <param name="location_id">Required: [String] Location ID</param>
        /// <param name="location_slug">Required: [String] Location slug</param>
        abstract protected string Sell_bitcoins_with_cash(string location_id, string location_slug);

        /// <summary>
        /// /buy-bitcoins-online/{countrycode:2}/{country_name}/{payment_method}/.json
        /// HTTP method: GET;
        /// - This API returns buy Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// Countrycodes are always exactly two characters (/api/countrycodes/).  Currencies (/api/currencies/) are always exactly three characters. Anything longer than that is a payment method. country_name is the full name of the country with _ instead of spaces.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="countrycode">Required: [String] Country code</param>
        /// <param name="country_name">Required: [String] Country name</param>
        /// <param name="payment_method">Required: [String] Payment method</param>
        abstract protected string Buy_bitcoins_online_by_country_and_payment(string countrycode, string country_name, string payment_method);

        /// <summary>
        /// /buy-bitcoins-online/{countrycode:2}/{country_name}/.json
        /// HTTP method: GET;
        /// - This API returns buy Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// countrycode is exactly two characters. country_name is the full name of the country with _ instead of spaces.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="countrycode">Required: [String] Country code</param>
        /// <param name="country_name">Required: [String] Country name</param>
        abstract protected string Buy_bitcoins_online_by_country(string countrycode, string country_name);

        /// <summary>
        /// /buy-bitcoins-online/{currency:3}/{payment_method}/.json
        /// HTTP method: GET;
        /// - This API returns buy Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// Currencies (/api/currencies/) are always exactly three characters. Anything longer than that is a payment method.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="currency">Required: [String] Currency (/api/currencies/)</param>
        /// <param name="payment_method">Required: [String] Payment method</param>
        abstract protected string Buy_bitcoins_online_by_currency_and_payment(string currency, string payment_method);

        /// <summary>
        /// /buy-bitcoins-online/{currency:3}/.json
        /// HTTP method: GET;
        /// - This API returns buy Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// Currencies (/api/currencies/) are always exactly three characters. Anything longer than that is a payment method.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="currency">Required: [String] Currency (/api/currencies/)</param>
        abstract protected string Buy_bitcoins_online_by_currency(string currency);

        /// <summary>
        /// /buy-bitcoins-online/{payment_method}/.json
        /// HTTP method: GET;
        /// - This API look up buy Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="payment_method">Required: [String] Payment method</param>
        abstract protected string Buy_bitcoins_online_by_payment(string payment_method);

        /// <summary>
        /// /buy-bitcoins-online/.json
        /// HTTP method: GET;
        /// - This API returns buy Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended. Ads are returned in the same structure as /api/ads/.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        abstract protected string Buy_bitcoins_online { get; }
        
        /// <summary>
        /// /sell-bitcoins-online/{countrycode:2}/{country_name}/{payment_method}/.json
        /// HTTP method: GET;
        /// - This API returns sell Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// Countrycodes (/api/countrycodes/) are always exactly two characters. Currencies (/api/currencies/) are always exactly three characters. Anything longer than that is a payment method. country_name is the full name of the country with _ instead of spaces.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="countrycode">Required: [String] Country code</param>
        /// <param name="country_name">Required: [String] Country name</param>
        /// <param name="payment_method">Required: [String] Payment method</param>
        abstract protected string Sell_bitcoins_online_by_country_and_payment(string countrycode, string country_name, string payment_method);

        /// <summary>
        /// /sell-bitcoins-online/{countrycode:2}/{country_name}/.json
        /// HTTP method: GET;
        /// - This API returns sell Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// countrycode is exactly two characters. country_name is the full name of the country with _ instead of spaces.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="countrycode">Required: [String] Country code</param>
        /// <param name="country_name">Required: [String] Country name</param>
        abstract protected string Sell_bitcoins_online_by_country(string countrycode, string country_name);

        /// <summary>
        /// /sell-bitcoins-online/{currency:3}/{payment_method}/.json
        /// HTTP method: GET;
        /// - This API returns sell Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// Currencies (/api/currencies/) are always exactly three characters. Anything longer than that is a payment method.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="currency">Required: [String] Currency (/api/currencies/)</param>
        /// <param name="payment_method">Required: [String] Payment method</param>
        abstract protected string Sell_bitcoins_online_by_currency_and_payment(string currency, string payment_method);

        /// <summary>
        /// /sell-bitcoins-online/{currency:3}/.json
        /// HTTP method: GET;
        /// - This API returns sell Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// Currencies (/api/currencies/) are always exactly three characters. Anything longer than that is a payment method.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="currency">Required: [String] Currency (/api/currencies/)</param>
        abstract protected string Sell_bitcoins_online_by_currency(string currency);

        /// <summary>
        /// /sell-bitcoins-online/{payment_method}/.json
        /// HTTP method: GET;
        /// - This API look up sell Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        /// <param name="payment_method">Required: [String] Payment method</param>
        abstract protected string Sell_bitcoins_online_by_payment(string payment_method);

        /// <summary>
        /// /sell-bitcoins-online/.json
        /// HTTP method: GET;
        /// - This API returns sell Bitcoin online ads. It is closely modeled after the online ad listings on LocalBitcoins.com. It occupies the same URLs with .json appended. Ads are returned in the same structure as /api/ads/.
        /// An example of a valid payment_method argument is national-bank-transfer. See /api/payment_methods/ for a list of all valid payment methods.
        /// Ads are returned in the same structure as /api/ads/.
        /// </summary>
        abstract protected string Sell_bitcoins_online { get; }

        /// <summary>
        /// /bitcoinaverage/ticker-all-currencies/
        /// HTTP method: GET;
        /// This API returns a ticker-tape like list of all completed trades.
        /// If there are no trades to calculate average for some time frame, then that average is not present.
        /// </summary>
        abstract protected string Bitcoinaverage_ticker_all_currencies { get; }

        /// <summary>
        /// /bitcoincharts/{currency}/trades.json
        /// HTTP method: GET;
        /// - All closed trades in online buy and online sell categories, updated every 15 minutes.
        /// The maximum batch size is of a fetch is 500 entries.Use? since = parameter with the last tid to iterate more.
        /// </summary>
        /// <param name="currency">Required: [String] Currency (/api/currencies/)</param>
        abstract protected string Bitcoincharts_trades_by_currency(string currency);

        /// <summary>
        /// /bitcoincharts/{currency}/orderbook.json
        /// HTTP method: GET;
        /// - Buy and sell bitcoin online advertisements. Amount is the maximum amount available for the trade request. Price is the hourly updated price. The price is based on the price equation and commission % entered by the ad author.
        /// No batching
        /// </summary>
        /// <param name="currency">Required: [String] Currency (/api/currencies/)</param>
        abstract protected string Bitcoincharts_orderbook_by_currency(string currency);
        #endregion
    }
}