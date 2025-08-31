using CsvHelper.Configuration;
using MasterRdsServices.Domain.Entities;

namespace MasterRdsServices.Domain.Mapping
{
    public class IcdMap: ClassMap<RecordsIcd>
    {
        public IcdMap()
        {
            Map(m => m.Id).Name("CODE");
            Map(m => m.DescriptionShort).Name("SHORT_DESCRIPTION");
            Map(m => m.DescriptionLong).Name("LONG_DESCRIPTION");
        }
    }
}
