using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.DataAccessLayer.Base
{
    public class FactoryBase
    {
        private string _ConnectionString = string.Empty;

        public string ConnectionString
        {
            get
            {
                if (_ConnectionString == string.Empty)
                {
                    _ConnectionString =
                        "Server=sql.decinfo-cchic.ca;Port=33306;Database=h24_esp_projet_1336289;Uid=dev-1336289;Pwd=Alarme123";
                }
                return _ConnectionString;
            }
        }
    }
}
