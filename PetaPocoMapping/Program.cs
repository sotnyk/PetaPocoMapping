using Models;
using PetaPoco;
using PetaPoco.Core.Inflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaPocoMapping
{
    class Program
    {
        static EnumMapper _enumMapper = new EnumMapper();
        static void Main(string[] args)
        {
            Mappers.Register(typeof(JsonEnumMap), ConstructEnumJsonMapper());

            using (var db = new Database())
            {
                db.Execute("TRUNCATE \"JsonEnumMap\"");
                for (int i = 0; i < 10; ++i)
                {
                    JsonEnumMap jsonEnumMap = new JsonEnumMap()
                    {
                        Jsonb = @"{""red"":""#f00"", ""green"":""#0f0"", ""n"":" + i + "}",
                        Event = EventTypes.PatientDeactivated,
                    };

                    db.Insert("JsonEnumMap", jsonEnumMap);
                }

                foreach(var row in db.Query<JsonEnumMap>("SELECT * FROM \"JsonEnumMap\" LIMIT 10"))
                {
                    Console.WriteLine($"Id: {row.Id}, Jsonb: {row.Jsonb}, Event: {row.Event}");
                }

                foreach (var row in db.Fetch<JsonEnumMap>("SELECT * FROM \"JsonEnumMap\" LIMIT 10"))
                {
                    row.Jsonb = @"{""changed"":true}";
                    db.Update(row);
                }

                foreach (var row in db.Query<JsonEnumMap>("SELECT * FROM \"JsonEnumMap\" LIMIT 10"))
                {
                    Console.WriteLine($"Id: {row.Id}, Jsonb: {row.Jsonb}, Event: {row.Event}");
                }


            }

        }

        static IMapper ConstructEnumJsonMapper()
        {
            var result = new ConventionMapper();

            var oldMapper = result.MapColumn;

            result.ToDbConverter = sourceProperty =>
            {
                if (sourceProperty.PropertyType.IsEnum)
                {
                    return en => _enumMapper.StringFromEnum(en);
                }
                return null;
            };
            result.FromDbConverter = (targetProperty, sourceType) =>
            {
                if (targetProperty.PropertyType.IsEnum)
                {
                    return str => _enumMapper.EnumFromString(targetProperty.PropertyType, str.ToString());
                }
                return null;
            };
            result.MapColumn = (ci, t, pi) =>
            {
                if (pi.PropertyType.IsEnum)
                {
                    ci.ColumnName = result.InflectColumnName(Inflector.Instance, pi.Name);
                    ci.InsertTemplate = "CAST({0}{1} AS \"EventTypes\")";
                    ci.UpdateTemplate = "{0} = CAST({1}{2} AS \"EventTypes\")";
                    return true;
                }
                if (pi.PropertyType==typeof(string) && pi.Name.EndsWith("Jsonb", StringComparison.InvariantCultureIgnoreCase))
                {
                    ci.ColumnName = result.InflectColumnName(Inflector.Instance, pi.Name);
                    ci.InsertTemplate = "CAST({0}{1} AS jsonb)";
                    ci.UpdateTemplate = "{0} = CAST({1}{2} AS jsonb)";
                    return true;
                }
                return oldMapper(ci, t, pi);
            };

            return result;
        }
    }

}
