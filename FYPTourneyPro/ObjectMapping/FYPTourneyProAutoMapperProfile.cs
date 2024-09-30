using AutoMapper;
using FYPTourneyPro.Entities.Books;
using FYPTourneyPro.Services.Dtos.Books;

namespace FYPTourneyPro.ObjectMapping;

public class FYPTourneyProAutoMapperProfile : Profile
{
    public FYPTourneyProAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<BookDto, CreateUpdateBookDto>();
        /* Create your AutoMapper object mappings here */
    }
}
