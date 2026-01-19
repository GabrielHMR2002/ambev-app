## Testing the API Using Visual Studio (Detailed Guide)

Visual Studio provides an integrated way to run **unit and integration tests** for your API, which is especially useful for verifying that your backend logic works correctly before deploying.

---

### 1. **Open Test Explorer**

* From the top menu, go to:
  `Test → Test Explorer`
  or use the shortcut: **Ctrl + E, T**.
* **Test Explorer** displays all detected tests in your solution, organized by project and namespace (e.g., `Unit.Application.Sales`, `Unit.Domain.Validation`).
* Tests are grouped hierarchically, making it easy to run a single test, a class of tests, or all tests in a project.
<img width="1918" height="568" alt="image" src="https://github.com/user-attachments/assets/3917adb5-bfab-4cb3-9433-4e020609b7aa" />
---

### 2. **Running Tests**

There are multiple ways to execute tests:

#### a) From Test Explorer:

* Right-click a test or group of tests → select:

  * **Run** → Runs the test(s) normally.
  * **Debug** → Runs the test(s) in debug mode to step through the code.
  * **Run Until Failure** → Keeps running tests until one fails.
* **Keyboard shortcuts:**

  * `Ctrl + R, T` → Run selected test(s)
  * `Ctrl + R, Ctrl + T` → Debug selected test(s)
  * `Ctrl + R, U` → Run Until Failure

#### b) From Test Menu:

* Go to `Test` → options like:

  * **Run All Tests** (`Ctrl + R, A`)
  * **Debug All Tests** (`Ctrl + R, Ctrl + A`)
  * **Clear All Test Results** (`Ctrl + R, Del`)

---

### 3. **Understanding Test Results**

After running tests, Test Explorer shows:

| Icon    | Meaning                      |
| ------- | ---------------------------- |
| ✅ Green | Passed test(s)               |
| ❌ Red   | Failed test(s)               |
| ⚪ Blue  | Skipped/Inconclusive test(s) |

* Clicking a test displays details in the **Output window**, including:

  * Exceptions (e.g., `AutoMapper.AutoMapperMappingException`)
  * Stack traces
  * Duration and build status

> Example from screenshots:
> 36 tests ran, **36 passed, 0 failed, 0 skipped**.
<img width="1918" height="1009" alt="image" src="https://github.com/user-attachments/assets/0a9358a5-dbff-46ca-8292-e4296e5df345" />
---

### 4. **Debugging Failing Tests**

* Right-click a failing test → select **Debug**.
* Visual Studio opens the test in **debug mode**.
* Set **breakpoints** in your API or handler code.
* Step through the code to inspect variables and logic.

---

### 5. **Organizing and Filtering Tests**

* **Expand/Collapse** test groups to focus on specific areas.
* **Filter tests** by project, namespace, or outcome using the toolbar.
* **Add to playlist** to group frequently run tests for convenience.



