using SkiResortRental.Domain;
using SkiResortRental.Services;
using SkiResortRental.Observers;
using SkiResortRental.Interfaces;
using SkiResortRental.Strategies;

//podstawowy dataset
var inventory = new List<InventoryItem>
{
    // ===== BUTY =====
    new InventoryItem(new Boots("Salomon Boots", 40), 8),
    new InventoryItem(new Boots("Salomon Boots", 42), 12),
    new InventoryItem(new Boots("Salomon Boots", 44), 6),
    new InventoryItem(new Boots("Atomic Hawx", 41), 5),
    new InventoryItem(new Boots("Atomic Hawx", 43), 7),

    // ===== KASKI =====
    new InventoryItem(new Helmet("Uvex Helmet", 54), 6),
    new InventoryItem(new Helmet("Uvex Helmet", 56), 8),
    new InventoryItem(new Helmet("Uvex Helmet", 58), 5),
    new InventoryItem(new Helmet("Giro Helmet", 55), 4),
    new InventoryItem(new Helmet("Giro Helmet", 57), 6),

    // ===== NARTY =====
    new InventoryItem(new Ski("Atomic Redster", 160), 4),
    new InventoryItem(new Ski("Atomic Redster", 170), 6),
    new InventoryItem(new Ski("Atomic Redster", 180), 3),
    new InventoryItem(new Ski("Rossignol Hero", 165), 5),
    new InventoryItem(new Ski("Rossignol Hero", 175), 4),

    // ===== SNOWBOARDY =====
    new InventoryItem(new Snowboard("Burton Custom", 155), 5),
    new InventoryItem(new Snowboard("Burton Custom", 162), 4),
    new InventoryItem(new Snowboard("Capita DOA", 158), 6),

    // ===== GOGLE =====
    new InventoryItem(new Googles("Oakley Flight Deck"), 10),
    new InventoryItem(new Googles("Anon M4"), 7),
    new InventoryItem(new Googles("Smith I/O Mag"), 6)
};


//===================== Główna pętla =====================

var clients = new List<Client>();
var statsService = new InventoryStatisticsService();

var financeService = new FinanceService();
IPricingStrategy pricingStrategy = new StandardSeasonPricing(); // Ustawiony sezon niski
var rentalService = new RentalService(pricingStrategy);

rentalService.AddObserver(financeService);
rentalService.AddObserver(new ConsoleNotifier());

bool mRunning = true;

while (mRunning)
{
    // ================== MENU GŁÓWNE ==================
    int mainChoice = ReadMenuChoice(
        "\n\n=== WYPOŻYCZALNIA NARCIARSKA ===\n\n" +
        "1. Panel magazynu\n" +
        "2. Panel sprzedawcy\n" +
        "0. Wyjście\n",
        0,
        2
    );
    
    switch (mainChoice)
    {
        // ================== PANEL MAGAZYNU ==================
        case 1:
            RunInventoryMenu(inventory, statsService);
            break;
            

        // ================== PANEL SPRZEDAWCY ==================
        case 2:
            RunSellerMenu(inventory,clients,statsService,rentalService,financeService);
            break;

        case 0:
            Console.Write("\nŻegnaj!");
            mRunning = false;
            break;
    }
}
//===================== Inne menu =====================
//magazyn
static void RunInventoryMenu(List<InventoryItem> inventory, InventoryStatisticsService statsService)
{
    bool iRunning = true;

    while (iRunning)
    {
        var global = statsService.GetGlobalStats(inventory);
        int iChoice = ReadMenuChoice(
            "\n\n=== PANEL MAGAZYNU ===\n\n" +
            $"STATYSTYKI:  | Wszystkie: {global.total}  ||  Dostępne: {global.available}  ||  Wypożyczone: {global.rented} |\n\n"+
            "1. Dodaj sprzęt\n" +
            "2. Usuń sprzęt\n" +
            "3. Lista sprzętu\n"+
            "0. Powrót\n",
            0,
            3
        );
        
        switch (iChoice)
        {
            case 1:
                AddEquipment(inventory);
                break;

            case 2:
                RemoveEquipment(inventory);
                break;

            case 3:
                ShowInventory(inventory);
                break;

            case 0:
                iRunning = false;
                break;
        }
    }    
    
}

//sprzedawca
static void RunSellerMenu(List<InventoryItem> inventory,List<Client> clients, InventoryStatisticsService statsService,RentalService rentalService,FinanceService financeService)
{
    bool sRunning = true;

    while (sRunning)
    {
        int sChoice = ReadMenuChoice(
            "\n\n=== PANEL SPRZEDAWCY ===\n\n" +
            "STATYSTYKI:  | "+
            $"Dostępne sztuki: {inventory.Sum(i => i.AvailableQuantity)}"+
            $"  ||  Wypożyczone sztuki: {inventory.Sum(i => i.RentedQuantity)}"+
            $"  ||  Aktywni klienci: {rentalService.ActiveRentals.Select(r => r.Client).Distinct().Count()}"+
            $"  ||  Zarobek: {financeService.Earned} zł"+
            $"  ||  Kaucje: {financeService.Deposits} zł |\n\n"+
            "1. Wypożycz sprzęt\n" +
            "2. Zwróć sprzęt\n" +
            "3. Aktualne wypożyczenia\n"+
            "0. Powrót\n",
            0,
            3
        );
        
        switch (sChoice)
        {
            case 1:
                RentEquipment(inventory, clients, rentalService);
                break;

            case 2:
                ReturnEquipment(rentalService);
                break;

            case 3:
                ShowActiveRentals(rentalService);
                break;

            case 0:
                sRunning = false;
                break;
        }
    }
}


//===================== Funkcje pomocnicze =====================

//wybor kategori
static int SelectCategory(bool allowAll)
{
    while (true)
    {
        Console.WriteLine("=== WYBIERZ KATEGORIĘ ===\n");
        Console.WriteLine("1. Narty");
        Console.WriteLine("2. Snowboardy");
        Console.WriteLine("3. Buty");
        Console.WriteLine("4. Kaski");
        Console.WriteLine("5. Gogle");

        if (allowAll)
            Console.WriteLine("6. Wszystkie");

        Console.WriteLine("0. Powrót\n");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (choice == 0)
                return 0;

            if (choice >= 1 && choice <= 5)
                return choice;

            if (allowAll && choice == 6)
                return 6;
        }

        Console.WriteLine("\nNieprawidłowy wybór kategorii.\n\n");
    }
}

//mapowanie kategori
static List<InventoryItem> FilterInventoryByCategory(List<InventoryItem> inventory, int category)
{
    return category switch
    {
        1 => inventory.Where(i => i.Equipment is Ski).ToList(),
        2 => inventory.Where(i => i.Equipment is Snowboard).ToList(),
        3 => inventory.Where(i => i.Equipment is Boots).ToList(),
        4 => inventory.Where(i => i.Equipment is Helmet).ToList(),
        5 => inventory.Where(i => i.Equipment is Googles).ToList(),
        6 => inventory.ToList(),
        _ => new List<InventoryItem>()
    };
}

//wyswietlanie detali
static string GetEquipmentDetails(Equipment equipment)
{
    return equipment switch
    {
        Ski ski => $"{ski.Length} cm",
        Snowboard board => $"{board.Length} cm",
        Boots boots => $"EU {boots.Size}",
        Helmet helmet => helmet.Size.ToString(),
        _ => string.Empty
    };
}

//walidacja int
static int ReadInt(string message)
{
    int value;
    while (true)
    {
        Console.Write(message);
        if (int.TryParse(Console.ReadLine(), out value))
            return value;

        Console.WriteLine("\nNieprawidłowa wartość. Spróbuj ponownie.");
    }
}

//walidacja zakresu int
static int ReadIntInRange(string message, int min, int max)
{
    int value;
    while (true)
    {
        Console.Write(message);
        if (int.TryParse(Console.ReadLine(), out value) &&
            value >= min && value <= max)
            return value;

        Console.WriteLine($"\nPodaj liczbę z zakresu {min}–{max}.");
    }
}

//walidacje string
static string ReadNonEmptyString(string message)
{
    while (true)
    {
        Console.Write(message);
        var input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
            return input;

        Console.WriteLine("\nWartość nie może być pusta.");
    }
}

//walidacje menu
static int ReadMenuChoice(string title, int min, int max)
{
    while (true)
    {
        Console.WriteLine(title);

        if (int.TryParse(Console.ReadLine(), out int choice) &&
            choice >= min && choice <= max)
            return choice;

        Console.WriteLine("\nNieprawidłowy wybór. Spróbuj ponownie.\n");
    }
}

//===================== Funkcje pomocnicze magazyn =====================


//wyświetlanie
static void ShowInventory(List<InventoryItem> inventory)
{
    Console.WriteLine("\n\n=== LISTA SPRZĘTU ===");
    int category = SelectCategory(true);

    if (category == 0) return; 
    
    var filtered = FilterInventoryByCategory(inventory, category);

    Console.WriteLine("\n\n=== LISTA SPRZĘTU ===\n");

    if (!filtered.Any())
    { 
        Console.WriteLine("Brak sprzętu w tej kategorii.");
        return;
    }

    int index = 1;
    foreach (var item in filtered)
    { 
        var details = GetEquipmentDetails(item.Equipment);
        var detailsText = string.IsNullOrEmpty(details) ? "" : $" ({details})";

        Console.WriteLine(
            $"{index++}. {item.Equipment.Name}{detailsText} | " +
            $"Łącznie: {item.TotalQuantity}, " +
            $"Dostępne: {item.AvailableQuantity}, " +
            $"Wypożyczone: {item.RentedQuantity}"
        );
    }
}

//dodawanie
static void AddEquipment(List<InventoryItem> inventory)
{
    Console.WriteLine("\n\n=== DODAWANIE SPRZĘTU ===");
    int category = SelectCategory(allowAll: false);

    if (category == 0) return;

    Equipment equipment;

    switch (category)
    {
        case 1: // NARTY
            string skiName = ReadNonEmptyString("Nazwa nart: ");
            int length = ReadIntInRange("Długość (cm): ", 100, 220);
            equipment = new Ski(skiName, length);
            break;

        case 2: // SNOWBOARD
            string boardName = ReadNonEmptyString("Nazwa snowboardu: ");
            int boardLength = ReadIntInRange("Długość (cm): ", 130, 200);

            equipment = new Snowboard(boardName, boardLength);
            break;

        case 3: // BUTY
            string bootsName = ReadNonEmptyString("Nazwa butów: ");
            int size = ReadIntInRange("Rozmiar EU: ", 30, 50);

            equipment = new Boots(bootsName, size);
            break;

        case 4: // KASK
            string helmetName = ReadNonEmptyString("Nazwa kasku: ");
            int helmetSize = ReadIntInRange("Rozmiar kasku (1–6): ", 1, 6);
            
            equipment = new Helmet(helmetName, helmetSize);
            break;

        case 5: // GOGLE
            string gogglesName = ReadNonEmptyString("Nazwa gogli: ");
            
            equipment = new Googles(gogglesName);
            break;

        default:
            Console.WriteLine("Nieprawidłowa kategoria.");
            return;
    }
    
    int quantity = ReadIntInRange("\nPodaj ilość: ", 1, 1000);


    // sprawdzamy, czy taki sprzęt już istnieje
    var existing = inventory.FirstOrDefault(i =>
        IsSameEquipment(i.Equipment, equipment)
    );

    if (existing != null)
    {
        existing.AddQuantity(quantity);
        Console.WriteLine("\nZwiększono ilość istniejącego sprzętu.");
    }
    else
    {
        inventory.Add(new InventoryItem(equipment, quantity));
        Console.WriteLine("\nDodano nowy sprzęt do magazynu.");
    }
}

            
//usuwanie
static void RemoveEquipment(List<InventoryItem> inventory)
{
    Console.WriteLine("\n\n=== USUWANIE SPRZĘTU ===");
    int category = SelectCategory(allowAll: false);

    if (category == 0) return;

    var filtered = FilterInventoryByCategory(inventory, category);

    if (!filtered.Any())
    {
        Console.WriteLine("\nBrak sprzętu w tej kategorii.");
        return;
    }

    Console.WriteLine("\nDostępny sprzęt:\n");

    for (int i = 1; i <= filtered.Count; i++)
    {
        var item = filtered[i-1];
        var details = GetEquipmentDetails(item.Equipment);
        var detailsText = string.IsNullOrEmpty(details) ? "" : $" ({details})";

        Console.WriteLine(
            $"{i}. {item.Equipment.Name}{detailsText} | " +
            $"Łącznie: {item.TotalQuantity}, " +
            $"Dostępne: {item.AvailableQuantity}, " +
            $"Wypożyczone: {item.RentedQuantity}"
        );
    }

    int index = ReadIntInRange("\nWybierz indeks sprzętu do usunięcia: ", 1, filtered.Count);
    
    var selectedItem = filtered[index-1];
    
    int amount = ReadIntInRange(
        $"Podaj ilość do usunięcia (max {selectedItem.AvailableQuantity}): ",
        1,
        selectedItem.AvailableQuantity
    );
    
    selectedItem.RemoveQuantity(amount);

    if (selectedItem.TotalQuantity == 0)
        inventory.Remove(selectedItem);

    Console.WriteLine("\nSprzęt usunięty poprawnie.");
}

//porównywanie
static bool IsSameEquipment(Equipment a, Equipment b)
{
    if (a.GetType() != b.GetType())
        return false;

    if (a.Name != b.Name)
        return false;

    return (a, b) switch
    {
        (Ski s1, Ski s2) => s1.Length == s2.Length,
        (Snowboard s1, Snowboard s2) => s1.Length == s2.Length,
        (Boots b1, Boots b2) => b1.Size == b2.Size,
        (Helmet h1, Helmet h2) => h1.Size == h2.Size,
        _ => true // np. Googles
    };
}

//===================== Funkcje pomocnicze sprzedawca =====================

//wypozyczenie
static void RentEquipment(List<InventoryItem> inventory, List<Client> clients, RentalService rentalService)
{
    Console.WriteLine("\n\n=== WYPOŻYCZENIE SPRZĘTU ===\n");

    string name = ReadNonEmptyString("Imię i nazwisko klienta: ");

    var client = clients.FirstOrDefault(c => c.Name == name)
                 ?? new Client(name);

    if (!clients.Contains(client))
        clients.Add(client);

    while (true)
    {
        int category = SelectCategory(allowAll: false);
        
        if (category == 0)
            break;

        var filtered = FilterInventoryByCategory(inventory, category)
            .Where(i => i.AvailableQuantity > 0)
            .ToList();

        if (!filtered.Any())
        {
            Console.WriteLine("\nBrak dostępnego sprzętu w tej kategorii.");
            continue;
        }
        
        Console.WriteLine("\nDostępny sprzęt:\n");
        
        var dailyPrice = rentalService.GetDailyPrice(filtered[0].Equipment);
        var deposit = filtered[0].Equipment.Deposit;

        Console.WriteLine(
            $"\nCENA WYPOŻYCZENIA (SEZONOWA): {dailyPrice} zł / dzień || KAUCJA: {deposit} zł\n"
        );

        for (int i = 0; i < filtered.Count; i++)
        {
            var item = filtered[i];
            var details = GetEquipmentDetails(item.Equipment);
            var detailsText = string.IsNullOrEmpty(details) ? "" : $" ({details})";

            Console.WriteLine(
                $"{i + 1}. {item.Equipment.Name}{detailsText} | Dostępne: {item.AvailableQuantity}"
            );
        }
        
        Console.WriteLine("\n0. Powrót\n");
        
        int index = ReadIntInRange(
            "\nWybierz sprzęt: ",
            0,
            filtered.Count
        );
        
        if( index == 0) continue;
        
        rentalService.Rent(client, filtered[index - 1]);

        Console.WriteLine("\nSprzęt wypożyczony. Wrócono do wyboru kategorii.\n");
    }

    Console.WriteLine("\nZakończono proces wypożyczenia.");
}

//zwrot
static void ReturnEquipment(RentalService rentalService)
{
    Console.WriteLine("\n\n=== ZWROT SPRZĘTU ===\n");
    
    if (!rentalService.ActiveRentals.Any())
    {
        Console.WriteLine("Brak aktywnych wypożyczeń.");
        return;
    }


    var groupedByClient = rentalService.ActiveRentals
        .GroupBy(r => r.Client)
        .ToList();
    
    Console.WriteLine("Klienci z aktywnymi wypożyczeniami:\n");

    for (int i = 0; i < groupedByClient.Count; i++)
    {
        var group = groupedByClient[i];
        Console.WriteLine(
            $"{i + 1}. {group.Key.Name} (wypożyczenia: {group.Count()})"
        );
    }

    int clientIndex = ReadIntInRange(
        "\nWybierz klienta: ",
        1,
        groupedByClient.Count
    );

    var selectedGroup = groupedByClient[clientIndex - 1];
    var rentals = selectedGroup.ToList();
    
    if (rentals.Count == 1)
    {
        rentalService.Return(rentals[0]);
        Console.WriteLine("\nSprzęt zwrócony.");
        return;
    }
    
    Console.Write("\nKlient ma kilka wypożyczeń. Zwrócić wszystkie? (t/n): ");
    var answer = Console.ReadLine()?.Trim().ToLower();

    if (answer == "t")
    {
        foreach (var rental in rentals.ToList())
        {
            rentalService.Return(rental);
        }

        Console.WriteLine("\nWszystkie wypożyczenia zostały zwrócone.");
        return;
    }
    
    Console.WriteLine("\nWybierz wypożyczenie do zwrotu:\n");

    for (int i = 0; i < rentals.Count; i++)
    {
        var r = rentals[i];
        var details = GetEquipmentDetails(r.Item.Equipment);
        var detailsText = string.IsNullOrEmpty(details) ? "" : $" ({details})";

        Console.WriteLine(
            $"{i + 1}. {r.Item.Equipment.Name}{detailsText}"
        );
    }

    int rentalIndex = ReadIntInRange(
        "\nWybierz wypożyczenie: ",
        1,
        rentals.Count
    );

    rentalService.Return(rentals[rentalIndex - 1]);

    Console.WriteLine("\nSprzęt zwrócony.");
}

//aktywne wyporzyczenia
static void ShowActiveRentals(RentalService rentalService)
{
    Console.WriteLine("\n\n=== AKTUALNE WYPOŻYCZENIA ===\n");

    if (!rentalService.ActiveRentals.Any())
    {
        Console.WriteLine("Brak aktywnych wypożyczeń.");
        return;
    }

    foreach (var r in rentalService.ActiveRentals)
    {
        var details = GetEquipmentDetails(r.Item.Equipment);
        var detailsText = string.IsNullOrEmpty(details) ? "" : $" ({details})";

        Console.WriteLine(
            $"{r.Client.Name} → {r.Item.Equipment.Name}{detailsText}"
        );
    }
}

