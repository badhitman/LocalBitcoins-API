////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////

namespace LocalBitcoinsAPI
{
    abstract class lbMetadata
    {
        /////////////////////////////////////////////////
        //#region Public functions
        /// <summary>
        /// Смотрит места рядом широта, долгота и предоставляет полные URL-адреса, чтобы покупать и продавать списки для каждого из них.
        /// Вы можете использовать внешние сервисы, такие как Google Места, чтобы найти широта/долгота координаты для адресов и тому подобное.Таким образом, вы также можете получить CountryCode и location_string значения для улучшения вашего поиска.
        /// InvalidParameter(код ошибки 11) возвращается, если параметры не кажутся действительными.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="countrycode"></param>
        /// <param name="location_string"></param>
        /// <returns></returns>
        abstract protected string getPlaces_as_string(string lat, string lon, string countrycode = "", string location_string = "");

        /// <summary>
        /// Эти API выглядят локальные объявления. Вы можете дать Широта и долгота получить параметры, чтобы указать расположение вблизи {LOCATION_ID}, до 50 километров.
        /// getPlaces_as_string API полезно, чтобы получить {LOCATION_ID} и {location_slug} по получения широты/долготы.
        /// </summary>
        /// <param name="ad_type">Тип сделки: покупка (buy) или продажа (sell)</param>
        /// <param name="location_id"></param>
        /// <param name="location_slug"></param>
        /// <returns></returns>
        abstract protected string getLocalAdLookups_as_string(string ad_type, string location_id, string location_slug);
    }
}
