// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CodeBuilder.PDManer
{
    public class PdmParser
    {
        public static PdmDefinition Parse(string fileName)
        {
            var content = File.ReadAllText(fileName);
            var definition = JsonConvert.DeserializeObject<PdmDefinition>(content);
            if (definition.ViewGroups.Count > 0)
            {
                foreach (var view in definition.ViewGroups)
                {
                    view.Entities = view.RefEntities.Select(s => definition.Entities.FirstOrDefault(t => t.Id == s)).ToList();
                }
            }

            return definition;
        }
    }
}
