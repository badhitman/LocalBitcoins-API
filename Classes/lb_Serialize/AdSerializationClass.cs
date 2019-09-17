////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////

using MultiTool;
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class AdSerializationClass
    {
        [DataMember]
        public UserProfileSerializationClass profile;

        [DataMember]
        public int require_feedback_score;

        [DataMember]
        public bool hidden_by_opening_hours;

        /// <summary>
        /// boolean if LOCAL_SELL
        /// </summary>
        [DataMember]
        public bool floating;
        
        /// <summary>
        /// string, often one of LOCAL_SELL, LOCAL_BUY, ONLINE_SELL, ONLINE_BUY,
        /// </summary>
        [DataMember]
        public string trade_type;

        /// <summary>
        /// primary key of the ad
        /// </summary>
        [DataMember]
        public int ad_id;

        /// <summary>
        /// current price per BTC in USD
        /// </summary>
        [DataMember]
        public string temp_price;

        [DataMember]
        public string bank_name;

        [DataMember]
        public int payment_window_minutes;

        [DataMember]
        public bool trusted_required;

        /// <summary>
        /// string repr of a decimal or null
        /// </summary>
        [DataMember]
        public string min_amount;

        [DataMember]
        public bool visible;

        [DataMember]
        public bool require_trusted_by_advertiser;

        [DataMember]
        public string temp_price_usd;

        [DataMember]
        public string lat;

        [DataMember]
        public string lon;

        [DataMember]
        public string age_days_coefficient_limit;

        [DataMember]
        public bool is_local_office;

        /// <summary>
        /// string representation of a decimal or null
        /// </summary>
        [DataMember]
        public string first_time_limit_btc;

        [DataMember]
        public string atm_model;

        /// <summary>
        /// city name
        /// </summary>
        [DataMember]
        public string city;

        /// <summary>
        /// human-readable location identifier string
        /// </summary>
        [DataMember]
        public string location_string;

        /// <summary>
        /// countrycode string, two letters
        /// </summary>
        [DataMember]
        public string countrycode;

        [DataMember]
        public string currency;

        [DataMember]
        public string limit_to_fiat_amounts;

        [DataMember]
        public string created_at;

        /// <summary>
        /// string repr of a decimal or null
        /// </summary>
        [DataMember]
        public string max_amount;

        [DataMember]
        public bool is_low_risk;
        
        /// <summary>
        /// string, e.g. SHORT
        /// </summary>
        [DataMember]
        public string reference_type;
        
        [DataMember]
        public bool sms_verification_required;

        [DataMember]
        public double require_trade_volume;

        /// <summary>
        /// payment method string e.g. NATIONAL_BANK,
        /// </summary>
        [DataMember]
        public string online_provider;

        /// <summary>
        /// string repr of a decimal or null
        /// </summary>
        [DataMember]
        public string max_amount_available;

        [DataMember]
        public string msg;

        [DataMember]
        public bool require_identification;

        [DataMember]
        public string email;

        /// <summary>
        /// string repr of a decimal
        /// </summary>
        [DataMember]
        public string volume_coefficient_btc;

        public double get_min_amount_as_double()
        {
            return glob_tools.GetDoubleFromString(min_amount);
        }

        public double get_max_amount_as_double()
        {
            return glob_tools.GetDoubleFromString(max_amount);
        }

        public double get_temp_price()
        {
            return glob_tools.GetDoubleFromString(temp_price);
        }
    }
}
