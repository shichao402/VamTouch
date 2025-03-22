using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VamTouch.Core.Models
{
    public class VamMetaDependency
    {
        [JsonPropertyName("licenseType")]
        public string LicenseType { get; set; }
        
        [JsonPropertyName("dependencies")]
        public Dictionary<string, VamMetaDependency> Dependencies { get; set; } = new Dictionary<string, VamMetaDependency>();
    }

    public class VamMeta
    {
        [JsonPropertyName("licenseType")]
        public string LicenseType { get; set; }
        
        [JsonPropertyName("creatorName")]
        public string CreatorName { get; set; }
        
        [JsonPropertyName("packageName")]
        public string PackageName { get; set; }
        
        [JsonPropertyName("standardReferenceVersionOption")]
        public string StandardReferenceVersionOption { get; set; }
        
        [JsonPropertyName("scriptReferenceVersionOption")]
        public string ScriptReferenceVersionOption { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("credits")]
        public string Credits { get; set; }
        
        [JsonPropertyName("instructions")]
        public string Instructions { get; set; }
        
        [JsonPropertyName("promotionalLink")]
        public string PromotionalLink { get; set; }
        
        [JsonPropertyName("programVersion")]
        public string ProgramVersion { get; set; }
        
        [JsonPropertyName("contentList")]
        public List<string> ContentList { get; set; } = new List<string>();
        
        [JsonPropertyName("dependencies")]
        public Dictionary<string, VamMetaDependency> Dependencies { get; set; } = new Dictionary<string, VamMetaDependency>();
        
        [JsonPropertyName("customOptions")]
        public Dictionary<string, string> CustomOptions { get; set; } = new Dictionary<string, string>();
        
        [JsonPropertyName("hadReferenceIssues")]
        public string HadReferenceIssues { get; set; }
        
        [JsonPropertyName("referenceIssues")]
        public List<object> ReferenceIssues { get; set; } = new List<object>();
    }
} 