# AidaProfileManager

**Автор:** Aida

AidaProfileManager — университеттің төртінші апталық зертханалық жұмысына арналған ASP.NET Core MVC қосымшасы. Ол пайдаланушы профильдерін SQLite дерекқорында сақтап, енгізілген мәндерді браузерде және серверде тексереді.

## Технологиялар

- C# 14 және .NET 10
- ASP.NET Core MVC және Razor Views
- Entity Framework Core 10 және SQLite
- Data Annotations, `IValidatableObject`, `StrongPasswordAttribute`
- jQuery Validation және jQuery Unobtrusive Validation
- HTML5, CSS3, JavaScript және Bootstrap Icons SVG жиыны

## Мүмкіндіктер

- Dashboard нақты дерекқордағы жалпы, белсенді және белсенді емес профиль санын, соңғы бес жазбаны көрсетеді.
- Профильдерді қосу, көру, өңдеу және жою, аты, username немесе email бойынша іздеу, күйі бойынша сүзу.
- Қазақ тіліндегі икемді интерфейс, мобильді навигация, Light/Dark режимі және toast хабарламалары.
- Create және Edit пішіндері үшін жеке ViewModel қолданылады. Дерекқор моделі тікелей формаға берілмейді.
- Username мен нормаланған email үшін бірегей SQLite индекстері бар.
- Құпиясөз `PasswordHasher<Profile>` арқылы хэштеледі. Ашық құпиясөз бен растау мәні сақталмайды, өңдеу бетінде бұрынғы құпиясөз көрсетілмейді.
- Валидация зертханасында ережелерді тексеріп көруге болады. Ол профильді дерекқорға сақтамайды.
- POST әрекеттерінде anti-forgery token қолданылады, ал Razor көріністері HTML мәндерін автоматты түрде кодтайды.

## MVC құрылымы

`Models/Profile.cs` — дерекқордың domain model-і. `ViewModels/` ішіндегі Create, Edit, Details, Dashboard және тізім модельдері интерфейске қажетті деректерді тасымалдайды. `ProfilesController` HTTP сұрауларын өңдейді. Дерекқормен асинхронды жұмыс `Services/ProfileService` ішінде орындалады. `ApplicationDbContext` SQLite конфигурациясын және бірегей индекстерді анықтайды.

## Валидация

- `[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`, `[Compare]` және `[RegularExpression]` өріс мәндерін тексереді.
- Create және Edit ViewModel-дерінің `IValidatableObject.Validate()` әдісі 18 жасқа толмаған пайдаланушыдан Bio өрісінде кемінде 20 таңба талап етеді.
- `StrongPasswordAttribute` құпиясөздің ұзындығын, бас және кіші әріп, сан мен арнайы белгінің болуын серверде тексереді. `IClientModelValidator` осы ережені jQuery Unobtrusive Validation-ға қосады.
- Контроллерде `ModelState.IsValid` тексеріледі. Қайталанатын username немесе email үшін `ModelState.AddModelError` қолданылады. UNIQUE индекстер қатар келген сұрауларды да қорғайды.
- `_ValidationScriptsPartial.cshtml` клиенттік тексеріс скрипттерін жергілікті файлдардан жүктейді.

## Іске қосу

Windows PowerShell-де жоба түбіріне өтіп орындаңыз:

```powershell
dotnet restore
dotnet ef database update
dotnet run
```

Қажет болса EF Core құралын орнатыңыз:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.15
```

Жоба алғаш іске қосылғанда миграциялар автоматты түрде қолданылады. SQLite файлы `App_Data/aida-profiles.db` ішінде жасалады. `App_Data/` қалтасы мен дерекқор файлдары Git-ке қосылмайды.

Құрылымды тексеру:

```powershell
dotnet build
```

## Валидация нұсқаулығы

Sidebar ішіндегі **Валидация нұсқаулығы** беті Data Annotations, `ModelState`, `IValidatableObject`, арнайы validation attribute және клиент/сервер тексерістерінің рөлін түсіндіреді. Playground пішіні тексеріс нәтижесін көрсетеді және дерекқорға жазба қоспайды.

## Қауіпсіздік шектеулері

Бұл зертханалық демода аутентификация мен авторизация қосылмаған. Сондықтан профильдер тізіміндегі email және телефон нөмірлері жүйеге кірмей-ақ көрінеді. Қосымшаны ашық интернетке жарияламас бұрын ASP.NET Core Identity не басқа аутентификация және авторизация жүйесін қосып, деректерге қолжетімділікті шектеу қажет.

## GitHub-қа дайындау

README мен EF Core миграциялары репозиторийге кіреді. `.gitignore` құрастыру файлдарын, жеке IDE баптауларын, SQLite дерекқорын, `.env` файлдарын, логтар мен тест нәтижелерін Git-тен шығарады.
