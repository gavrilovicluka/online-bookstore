using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Bookstore.Domain.Entities;

[CollectionName("Roles")]
public class UserRole : MongoIdentityRole<Guid>
{
}