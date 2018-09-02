using Newtonsoft.Json;
using System;

namespace RecipeTracker.Api.Request
{
    public abstract class Request
    {
        [JsonIgnore]
        internal Guid CallerId { get; set; }
    }
}
