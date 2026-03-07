using AutoMapper;
using CesarBmx.CryptoWatcher.Application.Responses;
using CesarBmx.CryptoWatcher.Domain.Models;

namespace CesarBmx.CryptoWatcher.Application.Mappers
{
    public class LogMapper : Profile
    {
        public LogMapper()
        {
            CreateMap<Log, LogResponse>();
        }
    }
}
