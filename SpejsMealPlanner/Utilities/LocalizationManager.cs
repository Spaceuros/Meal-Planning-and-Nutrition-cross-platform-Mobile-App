using Microsoft.Maui.Controls;

namespace SpejsMealPlanner.Utilities;

public static class LocalizationManager
{
    public static void SetLanguage(bool isEnglish)
    {
        var res = Application.Current!.Resources;

        res["AppTitle"] = "SpejsMealPlanner";
        res["LoginSubtitle"] = isEnglish ? "Enter PIN code or continue solo" : "Unesi PIN kod ili nastavi samostalno";
        res["PinCode"] = isEnglish ? "PIN code" : "PIN kod";
        res["PinPlaceholder"] = isEnglish ? "xxxx (leave empty for Solo mode)" : "xxxx (ostavi prazno za Solo režim)";
        res["NoPinRegister"] = isEnglish ? "No PIN? Register" : "Nemaš PIN? Registruj se";
        res["TabHome"] = isEnglish ? "Home" : "Početna";
        res["TabMeals"] = isEnglish ? "Meals" : "Obroci";
        res["TabIngredients"] = isEnglish ? "Ingredients" : "Sastojci";
        res["PlanForToday"] = isEnglish ? "Your plan for today" : "Tvoj plan za danas";
        res["Logout"] = isEnglish ? "Log out" : "Odjavi se";
        res["Protein"] = isEnglish ? "Protein" : "Proteini";
        res["Carbs"] = isEnglish ? "Carbs" : "Ugljeni H.";
        res["Fats"] = isEnglish ? "Fats" : "Masti";
        res["WaterIntake"] = isEnglish ? "Water intake" : "Unos vode";
        res["EnteredMeals"] = isEnglish ? "Entered meals" : "Uneti obroci";
        res["NoMealsToday"] = isEnglish ? "No meals today" : "Još uvek nema obroka";
        res["DeleteMeal"] = isEnglish ? "Delete meal" : "Obriši obrok";
        res["CarbsFull"] = isEnglish ? "Carbohydrates" : "Uglj. hidrati";
        res["MealsManagerTitle"] = isEnglish ? "Meal Management" : "Upravljanje Obrocima";
        res["YourMeals"] = isEnglish ? "Your Meals" : "Tvoji Obroci";
        res["History"] = isEnglish ? "History" : "Istorija";
        res["EmptyMealList"] = isEnglish ? "List is empty" : "Lista je prazna";
        res["ClickToAddMeal"] = isEnglish ? "Click + to add new meal" : "Klikni na + da dodaš novi obrok";
        res["Edit"] = isEnglish ? "Edit" : "Izmeni";
        res["Delete"] = isEnglish ? "Delete" : "Obriši";
        res["SwipeLeftOptions"] = isEnglish ? "Swipe left for options" : "Prevuci ulevo za opcije";
        res["AddMealTitle"] = isEnglish ? "Meal Details" : "Detalji Obroka";
        res["BasicInfo"] = isEnglish ? "Basic info" : "Osnovni podaci";
        res["MealName"] = isEnglish ? "MEAL NAME" : "NAZIV OBROKA";
        res["MealNamePlaceholder"] = isEnglish ? "E.g. Oatmeal" : "Npr. Ovseni doručak";
        res["MealCategory"] = isEnglish ? "CATEGORY" : "KATEGORIJA";
        res["MealCategoryPlaceholder"] = isEnglish ? "E.g. Breakfast" : "Npr. Doručak";
        res["IngredientsInMeal"] = isEnglish ? "Ingredients in meal" : "Sastojci u obroku";
        res["Choose"] = isEnglish ? "Choose" : "Izaberi";
        res["Grams"] = isEnglish ? "Grams" : "Grami";
        res["NoIngredientsAdded"] = isEnglish ? "No ingredients added." : "Nema dodatih sastojaka.";
        res["IngredientsDatabase"] = isEnglish ? "Ingredients Database" : "Baza Sastojaka";
        res["AddNewIngredient"] = isEnglish ? "Add new ingredient (per 100g)" : "Dodaj novu namirnicu (na 100g)";
        res["IngredientNamePlaceholder"] = isEnglish ? "Name (e.g. Egg, Chicken...)" : "Naziv (npr. Jaje, Piletina...)";
        res["AddToDatabase"] = isEnglish ? "Add to database" : "Dodaj u bazu";
        res["IngredientsDatabaseEmpty"] = isEnglish ? "Ingredients database is empty." : "Baza sastojaka je prazna.";
        res["SelectMealTitle"] = isEnglish ? "Select meal" : "Izaberi obrok";
        res["YourMenu"] = isEnglish ? "Your Menu" : "Tvoj Meni";
        res["AddReadyMeal"] = isEnglish ? "Add a ready meal to your daily plan" : "Dodaj gotov obrok u svoj današnji plan";
        res["NoReadyMeals"] = isEnglish ? "No ready meals." : "Nema gotovih obroka.";
        res["ChefNotAddedMeals"] = isEnglish ? "Chef hasn't added any meals yet." : "Kuvar ti још nije dodao nijedan obrok.";
        res["HistoryTitle"] = isEnglish ? "Meal history" : "Istorija obroka";
        res["DeletedMeals"] = isEnglish ? "Deleted meals" : "Obrisani obroci";
        res["HistorySubtitle"] = isEnglish ? "We keep the last 5. Swipe left to restore meal to daily plan." : "Zadržavamo poslednjih 5. Prevuci ulevo da vratiš obrok u dnevni plan.";
        res["HistoryEmpty"] = isEnglish ? "History is empty" : "Istorija je prazna";
        res["Restore"] = isEnglish ? "Restore" : "Vrati";
        res["SwipeLeftToRestore"] = isEnglish ? "Swipe left to restore" : "Prevuci ulevo da vratiš";
        res["Back"] = isEnglish ? "← Back" : "← Nazad";
        res["RegisterTitle"] = isEnglish ? "Registration" : "Registracija";
        res["RegisterSubtitle"] = isEnglish ? "Choose role and create profile" : "Izaberi ulogu i napravi profil";
        res["Consumer"] = isEnglish ? "Consumer" : "Konzument";
        res["Chef"] = isEnglish ? "Chef" : "Kuvar";
        res["Name"] = isEnglish ? "Name" : "Ime";
        res["NamePlaceholder"] = isEnglish ? "Enter your name" : "Unesi svoje ime";
        res["ChooseConsumer"] = isEnglish ? "Choose consumer you cook for" : "Izaberi konzumenta za koga kuvaš";
        res["ChooseConsumerPlaceholder"] = isEnglish ? "Choose consumer..." : "Izaberi konzumenta...";
        res["PinRegisterLabel"] = isEnglish ? "PIN code (min. 4 digits)" : "PIN kod (min. 4 cifre)";
        res["SaveUser"] = isEnglish ? "Save user" : "Sačuvaj korisnika";
    }

    public static string Translate(string key, bool isEnglish)
    {
        var dict = new Dictionary<string, string>
        {
            { "Error", isEnglish ? "Error" : "Greška" },
            { "ErrorWrongPin", isEnglish ? "Incorrect PIN code. Try again." : "Pogrešan PIN kod. Pokušaj ponovo." },
            { "ErrorNamePinRequired", isEnglish ? "Name and PIN are required." : "Ime i PIN su obavezna polja." },
            { "ErrorPinLength", isEnglish ? "PIN must have at least 4 digits." : "PIN mora imati barem 4 cifre." },
            { "ErrorChefConsumerRequired", isEnglish ? "You must select a consumer you cook for." : "Moraš izabrati konzumenta za koga kuvaš." },
            { "ErrorPinInUse", isEnglish ? "This PIN is already in use. Choose another." : "Ovaj PIN je već u upotrebi. Izaberi drugi." },
            { "Success", isEnglish ? "Success" : "Uspeh" },
            { "SuccessRegistered", isEnglish ? "User successfully registered!" : "Korisnik je uspešno registrovan!" },
            { "Ok", "OK" },
            { "SoloButtonEmpty", isEnglish ? "I can do it myself (Solo)" : "Mogu sam sve (Solo)" },
            { "SoloButtonFilled", isEnglish ? "Access application" : "Pristupi aplikaciji" },
            { "Save", isEnglish ? "Save" : "Sačuvaj" },
            { "Update", isEnglish ? "Update" : "Ažuriraj" }
        };

        return dict.TryGetValue(key, out var val) ? val : key;
    }
}