////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using LocalBitcoinsAPI.Classes.lb_Serialize;
using System.Collections.Generic;

namespace LocalBitcoinsAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            LocalBitcoins_API lb_api = new LocalBitcoins_API("auth key", "auth secret"); // укажите свои учётные данные API отсюда https://localbitcoins.com/accounts/api/
            
            PlacesSerializationClass list_Places = lb_api.Places("53.950609", "40.475365");
            //AdListBitcoinsOnlineSerializationClass list_Ads = lb_api.AdsByFilter();
            Dictionary<string, TickerAllCurrenciesSerializationClass> list_TickerAllCurrencies = lb_api.TickerAllCurrencies;
            List<ClassCurrencies> list_currencies = lb_api.Currencies;
            Dictionary<string, PaymentMethodsSerializationClass> list_PaymentMethods = lb_api.PaymentMethods("RU");
            AdListBitcoinsOnlineSerializationClass sell_bitcoin_online = lb_api.SellBitcoinsOnline(null, null, "rub", "qiwi"); // коды валют берём из: Currencies
            sell_bitcoin_online = lb_api.SellBitcoinsOnline(null, null, "rub", null);
            AdListBitcoinsOnlineSerializationClass buy_bitcoin_online = lb_api.BuyBitcoinsOnline(null, null, "rub", "qiwi"); // коды валют берём из: Currencies

            TradeItemSerializationClass[] list_trades = lb_api.Trades("RUB");
            
            OrdersSerializationClass list_orders = lb_api.Orderbook("RUB");
            CountryCodesSerializationClass list_CountryCodes = (CountryCodesSerializationClass)lb_api.CountryCodes;
        }
    }
}
