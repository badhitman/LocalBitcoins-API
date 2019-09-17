////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System;
using System.Security.Cryptography;
using System.Text;

namespace LocalBitcoinsAPI
{
    class lbHTTPCore: lbHTTPxml
    {
        /////////////////////////////////////////////////
        #region HMAC functions
        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        private static string HashHMAC(string key, string message)
        {
            byte[] hash = HashHMAC(new ASCIIEncoding().GetBytes(key), new ASCIIEncoding().GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToUpper(); ;
        }
        #endregion

        #region fields
        

        public enum DashboardStates { Open, Released, Canceled, Closed };
        public enum DashboardFilter { Buyer, Seller, All };
        public enum FeedbackStatus { trust, positive, neutral, block, block_without_feedback };
        public enum LB_TransactionTypes { Send = 1, Pending, Other, Fees, Internal };
        public enum LB_TradeTypes { LOCAL_SELL, LOCAL_BUY, ONLINE_SELL, ONLINE_BUY, ALL };
        public enum LB_TradeVisibleStatus { No, Yes, All };

        /// <summary>
        /// Тип локальной сделки. Используется для фильтра в методе getLocalAdLookups_as_string
        /// </summary>
        protected enum LB_LocalAdType { buy, sell };
        #endregion

        public lbHTTPCore(string hmac_auth_key, string hmac_auth_secret)
        {
            my_hmac_auth_key = hmac_auth_key;
            my_hmac_auth_secret = hmac_auth_secret;
        }

        

        

        
    }
}

/*
 
    Online Ad Lookups
/buy-bitcoins-online/{countrycode:2}/{country_name}/{payment_method}/.json
/buy-bitcoins-online/{countrycode:2}/{country_name}/.json
/buy-bitcoins-online/{currency:3}/{payment_method}/.json
/buy-bitcoins-online/{currency:3}/.json
/buy-bitcoins-online/{payment_method}/.json
/buy-bitcoins-online/.json

/sell-bitcoins-online/{countrycode:2}/{country_name}/{payment_method}/.json
/sell-bitcoins-online/{countrycode:2}/{country_name}/.json
/sell-bitcoins-online/{currency:3}/{payment_method}/.json
/sell-bitcoins-online/{currency:3}/.json
/sell-bitcoins-online/{payment_method}/.json
/sell-bitcoins-online/.json

These APIs look up online ads. They are closely modeled to the HTML online ad listings, and in fact occupy the same URLs just with .json appended.

Countrycodes are always exactly two characters. Currencies are always exactly three characters. Anything longer than that is a payment method. It's fuzzy, but it's how our URLs work. Sorry.

An example of a valid payment_method argument is national-bank-transfer.

Ads are returned in the same structure as /api/ads/.

The following APIs are useful in enumerating possible ways to look up online ads.
 
 */
