# Sprout.Exam.WebApp

### Proposed Improvements
- [ ] Implement full automation for the calculation process.
    - [ ] Implement roles on account. Accounts should have roles (eg. admin, hr, employee). This is controll the access of the user on different pages.
    - [ ] Develop a dedicated login and logout page for employees.
        - To streamline the calculation process, the initial step involves the creation of a user-friendly login and logout page for employees. This necessitates the establishment of a new table specifically for employee accounts. The table should have the following fields (username, password, employeeid, email, contactNumber, role).

        - [ ] Create a new table store all the login/logout data. These datas are necessary for the calculation automation process. Log table should have following fields (EmployeeId, TimeIn, TimeOut)

    - [ ] Develop a process to calculate all the salary of all the employees.
        - [ ] Create a new table to store all the monthly salary computations. The table should have the following fields (EmployeeId, Month, Year, Salary).
        - [ ] Create new class for the salary computation of all the employees.
        - [ ] Create a user interface where the admin/hr can trigger the salary computation for all the employees and store it to the monthly salary computation table. This new page should only be access by the hr.
            - [ ] Create validation for the process to check if the account that triggers the computation has a role of hr.

    - [ ] Create a report page that is only accessible to hr. This is where hr can download all the salary computations.

    - [ ] Crate a new screen where employees can view or download their payslips.

