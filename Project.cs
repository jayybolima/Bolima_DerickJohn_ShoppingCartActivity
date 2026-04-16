using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public Product(int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        RemainingStock = stock;
    }

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock})");
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity <= RemainingStock;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}

class CartItem
{
    public Product Product;
    public int Quantity;
    public double Subtotal;

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
        Subtotal = product.GetItemTotal(quantity);
    }

    public void Update(int quantity)
    {
        Quantity += quantity;
        Subtotal = Product.GetItemTotal(Quantity);
    }
}

class Program
{
    static void Main()
    {
        Product[] store = new Product[]
        {
            new Product(1, "Laptop", 30000, 5),
            new Product(2, "Mouse", 500, 10),
            new Product(3, "Keyboard", 1500, 8),
            new Product(4, "Headset", 2000, 0)
        };

        CartItem[] cart = new CartItem[5];
        int cartCount = 0;

        string choice = "Y";

        do
        {
            Console.WriteLine("\n=== STORE MENU ===");
            foreach (Product p in store)
            {
                p.DisplayProduct();
            }

            Console.Write("\nEnter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int productNum) ||
                productNum < 1 || productNum > store.Length)
            {
                Console.WriteLine("Invalid product number.");
                continue;
            }

            Product selected = store[productNum - 1];

            if (selected.RemainingStock == 0)
            {
                Console.WriteLine("Product is out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (!selected.HasEnoughStock(qty))
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            bool found = false;

            for (int i = 0; i < cartCount; i++)
            {
                if (cart[i].Product.Id == selected.Id)
                {
                    cart[i].Update(qty);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full.");
                    continue;
                }

                cart[cartCount++] = new CartItem(selected, qty);
            }

            selected.DeductStock(qty);

            Console.WriteLine("Item added to cart!");

            Console.Write("Add another item? (Y/N): ");
            choice = Console.ReadLine().ToUpper();

        } while (choice == "Y");

        Console.WriteLine("\n=== RECEIPT ===");
        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine($"{cart[i].Product.Name} x{cart[i].Quantity} = ₱{cart[i].Subtotal}");
            grandTotal += cart[i].Subtotal;
        }

        Console.WriteLine($"Grand Total: ₱{grandTotal}");

        double discount = 0;

        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
            Console.WriteLine($"Discount: ₱{discount}");
        }

        double finalTotal = grandTotal - discount;
        Console.WriteLine($"Final Total: ₱{finalTotal}");

        // UPDATED STOCK
        Console.WriteLine("\n=== UPDATED STOCK ===");
        foreach (Product p in store)
        {
            p.DisplayProduct();
        }
    }
}
