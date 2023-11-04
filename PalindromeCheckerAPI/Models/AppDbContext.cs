using Microsoft.EntityFrameworkCore;

namespace PalindromeCheckerAPI.Models;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Palindrome> Palindromes { get; set; }
}
