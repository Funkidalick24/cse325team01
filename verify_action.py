from playwright.sync_api import sync_playwright, expect
import os
import time

def run():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        context = browser.new_context()
        page = context.new_page()

        try:
            print("Navigating to register...")
            page.goto("http://localhost:5000/register")

            test_email = f"testuser_{os.getpid()}@example.com"
            page.fill('input[id="email"]', test_email)
            page.fill('input[id="password"]', "Password123")
            page.fill('input[id="confirm-password"]', "Password123")

            print("Submitting registration...")
            page.click('button[type="submit"]')
            page.wait_for_url("http://localhost:5000/tasks", timeout=10000)

            print("Navigating to add task...")
            with page.expect_navigation():
                page.click('text="+ Add New Task"')

            print("Filling add task form...")
            page.fill('input[id="title"]', "Test Delete and Complete Task")
            page.fill('textarea[id="description"]', "This should be toggleable and deletable.")
            page.click('text="Work"')

            print("Submitting add task...")
            with page.expect_navigation():
                page.click('button[type="submit"]:has-text("Save")')

            print("Returned to tasks page.")
            expect(page.locator('.task-title').first).to_have_text("Test Delete and Complete Task")

            print("Toggling completion...")
            with page.expect_navigation():
                page.locator('.task-checkbox-btn').first.click()

            os.makedirs("/home/jules/verification", exist_ok=True)
            page.screenshot(path="/home/jules/verification/task_completed.png")

            print("Deleting task...")
            page.on("dialog", lambda dialog: dialog.accept())
            with page.expect_navigation():
                page.click('.btn-delete')

            expect(page.locator('.empty-state')).to_be_visible()
            page.screenshot(path="/home/jules/verification/task_deleted.png")
            print("Success! Verified delete and complete.")

        except Exception as e:
            print(f"Error: {e}")
            page.screenshot(path="/home/jules/verification/error_action.png")
            raise e
        finally:
            context.close()
            browser.close()

if __name__ == "__main__":
    run()
