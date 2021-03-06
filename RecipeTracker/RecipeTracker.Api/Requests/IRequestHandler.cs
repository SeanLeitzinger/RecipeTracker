﻿using System.Threading.Tasks;

namespace RecipeTracker.Api.Requests
{
    public interface IRequestHandlerAsync<request, response> where request : Request
    {
        Task<response> HandleAsync();
    }

    public interface IRequestHandlerAsync<request> where request : Request
    {
        Task HandleAsync();
    }

    public interface IRequestHandler<request, response> where request : Request
    {
        response Handle();
    }

    public interface IRequestHandler<request> where request : Request
    {
        void Handle();
    }
}
