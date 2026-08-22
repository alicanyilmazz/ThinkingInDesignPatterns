
using Behavioral.Patterns.Command;

Kitchen kitchen = new Kitchen();

Waiter waiter = new Waiter();

ICommand pizzaOrder = new PizzaOrderCommand(kitchen,"Alican");

ICommand burgerOrder = new BurgerOrderCommand(kitchen,"Mehmet");

waiter.TakeOrder(pizzaOrder);

Console.WriteLine();

waiter.TakeOrder(burgerOrder);