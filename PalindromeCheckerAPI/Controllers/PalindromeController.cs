using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalindromeCheckerAPI.Models;
using System.Text.RegularExpressions;

namespace PalindromeCheckerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PalindromeController : ControllerBase
{
	private readonly AppDbContext _context; // A field for the DbContext instance

	// A constructor that takes a AppDbContext parameter
	public PalindromeController(AppDbContext context)
	{
		_context = context;
	}

	// A GET method that returns all the palindromes from the database
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Palindrome>>> GetPalindromes()
	{
		// Use LINQ to query the Palindromes table and order by the creation date
		var palindromes = await _context.Palindromes.OrderBy(p => p.DateRegistered).ToListAsync();
		return Ok(palindromes); // Return a 200 OK response with the palindromes
	}

	// A POST method that takes a string parameter and checks if it is a palindrome
	[HttpPost]
	public async Task<ActionResult<Palindrome>> PostPalindrome(string text)
	{
		// Validate the input parameter
		if (string.IsNullOrEmpty(text))
		{
			return BadRequest("The text cannot be null or empty."); // Return a 400 Bad Request response with an error message
		}

		// Convert the text to lower case and remove any non-alphanumeric characters
		var normalizedText = Regex.Replace(text.ToLower(), "[^a-z0-9]", "");

		// Check if the normalized text is a palindrome by reversing it and comparing it with itself
		var reversedText = new string(normalizedText.Reverse().ToArray());
		var isPalindrome = normalizedText == reversedText;

		// If the text is not a palindrome, return a 400 Bad Request response with an error message
		if (!isPalindrome)
		{
			return BadRequest("The text is not a palindrome.");
		}

		//Prevent duplicates
		var existingPalindrome = await _context.Palindromes
										  .FirstOrDefaultAsync(p => p.Text == text);

		if (existingPalindrome != null)
		{
			return BadRequest("Palindrome already exists.");
		}

		// Create a new Palindrome instance with the text and the current date and time
		var palindrome = new Palindrome
		{
			Text = text,
			DateRegistered = DateTime.Now
		};

		// Add the palindrome to the Palindromes table and save the changes to the database
		_context.Palindromes.Add(palindrome);
		await _context.SaveChangesAsync();

		// Return a 201 Created response with the palindrome and the location header
		return CreatedAtAction(nameof(GetPalindrome), new { id = palindrome.Id }, palindrome);
	}

	// A GET method that takes an id parameter and returns the palindrome with that id
	[HttpGet("{id}")]
	public async Task<ActionResult<Palindrome>> GetPalindrome(int id)
	{
		// Use LINQ to query the Palindromes table and find the palindrome with the given id
		var palindrome = await _context.Palindromes.FindAsync(id);

		// If the palindrome is not found, return a 404 Not Found response
		if (palindrome == null)
		{
			return NotFound();
		}

		// Return a 200 OK response with the palindrome
		return Ok(palindrome);
	}
}
