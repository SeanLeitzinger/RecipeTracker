using Newtonsoft.Json;
using System;

namespace RecipeTracker.Api.Requests
{
    public abstract class Request
    {
        [JsonIgnore]
        internal Guid CallerId { get; set; }
    }
}
