1. **Fix missing `@formname` in `TaskCard.razor` forms**
   - The user is getting an error: "The POST request does not specify which form is being submitted...". This happens because Blazor 8 static forms require a `@formname` attribute when there are multiple forms, particularly in a loop.
   - I will modify `TodoApp/TodoApp/Components/Pages/Shared/TaskCard.razor` to include a unique `@formname` on both the toggle task `<form>` and delete task `<form>`, using the `Task.Id` to make it unique (e.g., `@formname="@($"toggle-{Task.Id}")"` and `@formname="@($"delete-{Task.Id}")"`).
2. **Review other forms**
   - I will check other forms in the application, such as in `Add.razor`, `Edit.razor`, `Login.razor`, and `Register.razor`, to ensure they are using `EditForm` with a `FormName` or have a `@formname` if they are plain HTML `<form>` elements. If they use `<EditForm>` with `FormName`, they are fine.
3. **Verify the application compiles and functions**
   - I will build the application using `dotnet build ./TodoApp/TodoApp.sln` to make sure there are no syntax errors.
4. **Complete pre commit steps**
   - I will run `pre_commit_instructions` to ensure proper testing, verifications, reviews and reflections are done.
5. **Submit the changes**
   - I will submit the changes.
