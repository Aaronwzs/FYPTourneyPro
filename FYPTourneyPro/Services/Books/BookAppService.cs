using System;
using FYPTourneyPro.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Services.Dtos.Books;
using FYPTourneyPro.Entities.Books;

namespace FYPTourneyPro.Services.Books;

public class BookAppService :
    CrudAppService<
        Book, //The Book entity
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto>, //Used to create/update a book
    IBookAppService //implement the IBookAppService
{
    public BookAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {
        GetPolicyName = FYPTourneyProPermissions.Books.Default;
        GetListPolicyName = FYPTourneyProPermissions.Books.Default;
        CreatePolicyName = FYPTourneyProPermissions.Books.Create;
        UpdatePolicyName = FYPTourneyProPermissions.Books.Edit;
        DeletePolicyName = FYPTourneyProPermissions.Books.Delete;
    }
}