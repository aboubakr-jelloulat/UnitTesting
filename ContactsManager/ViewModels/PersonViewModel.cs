namespace ContactsManager.ViewModels;

public class PersonViewModel
{
    public Guid Id { get; set; }

    public string PersonName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public double? Age { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public bool ReceiveNewsLetters { get; set; }
}