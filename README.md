# Integral Investing
This repo will contain the project for the CIS4891.0M1: SENIOR PROJECT class. It is an application called "Integral Investing" where users will be able to deposit funds and invest those funds into different stocks.

<table>
<caption>Table of Contents</caption>
<tr>
<td width="25%"">
	
[1. Overview](https://github.com/tommyv7806/integralinvesting#1-overview)
</td>
<td width="75%">Gives an overview of what the application does</td>
</tr>
<tr>
<td width="25%"">
	
[2. Solution Architecture](https://github.com/tommyv7806/integralinvesting#2-solution-architecture)
</td>
<td width="75%">High level overview of the application's architecture</td>
</tr>
<tr>
<td width="25%"">

[3. Wireframes](https://github.com/tommyv7806/integralinvesting#3-wireframes)
</td>
<td width="75%">Images that show the basic design/layout of the application</td>
</tr>
<tr>
<td width="25%"">
	
[4. User Stories](https://github.com/tommyv7806/integralinvesting?tab=readme-ov-file#4-user-stories)
</td>
<td width="75%">Provides examples from the user's perspective of what they need the application to do</td>
</tr>
<tr>
<td width="25%"">
	
[5. Use Cases](https://github.com/tommyv7806/integralinvesting?tab=readme-ov-file#5-use-cases)
</td>
<td width="75%">Several scenarios that describe how the user will use the software to achieve their goals</td>
</tr>
<tr>
<td width="25%"">
	
[6. Use Case Diagram](https://github.com/tommyv7806/integralinvesting?tab=readme-ov-file#6-use-case-diagram)
</td>
<td width="75%">Graphical representation of the list of use cases for this application</td>
</tr>
<tr>
<td width="25%"">
	
[7. Requirements](https://github.com/tommyv7806/integralinvesting?tab=readme-ov-file#7-requirements)
</td>
<td width="75%">A list of requirements that describe what the software shall do</td>
</tr>
<td width="25%"">
	
[8. Entity Relationship Diagram](https://github.com/tommyv7806/integralinvesting/blob/main/README.md#8-entity-relationship-diagram)
</td>
<td width="75%">Digram that displays the relationships between entities in the application</td>
</tr>
<td width="25%"">
	
[9. UML Diagram](https://github.com/tommyv7806/integralinvesting/blob/main/README.md#9-uml-diagram)
</td>
<td width="75%">Class diagram that shows the structure of the classes and objects that make up the application</td>
</tr>
<td width="25%"">
	
[10. Data Access Layer](https://github.com/tommyv7806/integralinvesting/blob/main/README.md#10-data-access-layer)
</td>
<td width="75%">Relevant information about the data access API for this application</td>
</tr>
</table>


# 1. Overview
### Creating an Account and Moving Money Around
	
The main purpose of this application is to allow users to deposit funds into their account, which they will then be able to use to invest in different stocks. At a high level, the main flow of the application will be something like this: First, users will register and create their login for the application. Their information will then be saved to the database, and this information will then be authenticated/retrieved any time they attempt to log in to the app. This will ideally give me some practice with setting up a secure authentication system for an application, as opposed to just saving the usernames/passwords in plaintext.
 
After the account has been created, users will then need to link one of their bank accounts with the application in order to deposit funds into their account. For the purpose of this project, this part will be simulated and the user won’t need to connect to any actual bank account. Once the bank account has been linked with the application account, users will then be able to move funds between the accounts. This means that not only will users be able to send money from their bank account, but they will also be able to send money from the application to their bank account.

### Working with an API to Retrieve Stock Information
	
Moving money in and out of the account is a necessary function, but the main functionality of this application will come from the investing portion. The funds that users have in their account will be available for purchasing stocks. The data for these stocks will come from an existing API that retrieves information from the stock market. So far, I have found several existing APIs that can be used to query the stock market data that will be able to cover my needs for this application. They all have some kind of free tier, which usually allows for a certain amount of API calls per minute or per day, which should be adequate for this kind of project. One potential way that I am hoping to maybe get around these limits is to try and use some kind of caching. Whenever the user logs into the application, one API call will be sent to retrieve the data for several hundred stocks (likely the most popular ones), and these results will be saved to the database. Then, whenever the user actually does a search to retrieve information about a particular stock, the application will query the cached data in the database instead of making live API calls. I believe this will also be adequate for this kind of project; however, in a real-life scenario, users would definitely want to make sure they have the most up-to-date pricing information.
	
Once the application sends the stock information back to the user, they will then be able to purchase an amount of that stock based on how much funds they have in their account. On the flip-side, whenever a user decides they want to sell some of their stocks, the application will either make a new call to the API, or use the cached data, to determine what the selling price would be. After the stocks have been sold, the money will then be deposited into the user’s account.
	
In addition to allowing users to be able to buy/sell stocks, as a stretch goal, I would also like to try and implement some kind of functionality for viewing a stock’s historical data. Of the APIs that I have reviewed, some do offer the ability to query historical data.

# 2. Solution Architecture
![Solution Architecture](https://github.com/tommyv7806/integralinvesting/assets/67933601/d4f2f1fb-af1a-4abd-83ff-d39008410ea7)

# 3. Wireframes
### Portfolio Page
![Portfolio Page Wireframe](https://github.com/tommyv7806/integralinvesting/assets/67933601/90145fd7-4727-4536-b4ea-42392fa8602c)

### Browse Page
![Browse Page Wireframe](https://github.com/tommyv7806/integralinvesting/assets/67933601/aa4eeff3-425f-4da3-9c78-dc849054a4c5)

### Account Page
![Account Page Wireframe](https://github.com/tommyv7806/integralinvesting/assets/67933601/d612ddc0-e8d8-4c1d-ab47-55abae70aca0)

# 4. English User Stories
1. Login Related
   - As a user, I need the ability to log in to the application using an external authentication service. 
   - As a user, I need the ability to maintain my user information.
2. Maintain Account Related
   - As a user, I need the ability to link my financial institution to my account so that I can deposit and withdraw funds.
3. Purchasing Related
   - As a user, I need the ability to query stock market data in order to get the latest pricing information for stocks. 
   - As a user, I need the ability to refine my query so that I can search for specific stocks.
   - As a user, I need the ability to use the funds in my account to purchase a selected stock.
4. Maintain Portfolio Related
   - As a user, I need the ability to view my portfolio so that I can see how my investments are doing at a glance.
   - As a user, I need the ability to refine/filter my portfolio data so that I can view data for particular stocks that I am invested in.
   - As a user, I need the ability to sell stocks that I own for the stock’s current price.

# 5. Use Cases
### Login
**Description:** As a user, I need the ability to log into my account using third party authentication services.

**Scenario 1: Login**
</br>&emsp;Given a login screen
</br>&emsp;When the user clicks the Login button
</br>&emsp;Then the user will be able to use third party authentication services to log into their account

**Scenario 2: Logout**
</br>&emsp;Given a logged in user
</br>&emsp;When the user clicks the Logout button
</br>&emsp;Then the user will be logged out of their account

### Maintain Account
**Description:** As a user, I need the ability to maintain my account within the app. This includes maintaining user information as well as adding/removing funds from the account.

**Background:** Given a user is logged in and on the Account Summary page

**Scenario 1: Maintain User Information**
</br>&emsp;When the user attempts to update their username
</br>&emsp;Then they will be able to update their username and save successfully

**Scenario 2: Add Funds to Account**
</br>&emsp;When the user clicks the Add Funds button
</br>&emsp;Then a dialog window will open where the user can withdraw funds from their financial institution

**Scenario 3: Remove Funds from Account**
</br>&emsp;When the user clicks the Remove Funds button
</br>&emsp;Then a dialog window will open where the user can deposit funds into their financial institution

### Purchase Stocks
**Description:** As a user, I need the ability to browse and purchase stocks to add to my portfolio.

**Scenario 1: Search for Stocks**
</br>&emsp;Given a user is on the Browse page
</br>&emsp;When the user types in a search term and clicks the Search button
</br>&emsp;Then the system will display a list of stocks that match the search term

**Scenario 2: Select Stock for Purchase**
</br>&emsp;Given a user has searched for a particular stock
</br>&emsp;When the user selects the stock
</br>&emsp;Then they can enter a quantity of stocks to purchase

**Scenario 3: Add Purchased Stocks to Portfolio**
</br>&emsp;Given a user has selected a stock 
</br>&emsp;&emsp;And the user has entered a purchase quantity
</br>&emsp;When the user clicks the Purchase button
</br>&emsp;Then the system will confirm that the user has enough funds in their account
</br>&emsp;&emsp;And the system will add the newly purchased fund to the user’s portfolio if they have enough funds for the purchase

### Maintain Portfolio
**Description:** As a user, I need the ability to view my portfolio and sell any stocks that I own.

**Scenario 1: View Portfolio**
</br>&emsp;Given a user is on the Portfolio page
</br>&emsp;Then the user will be able to see their portfolio with a breakdown of their stock investments

**Scenario 2: Refine Portfolio Display**
</br>&emsp;Given a user is on the Portfolio page
</br>&emsp;When the user adds a filter or sort to the portfolio
</br>&emsp;Then the portfolio data will be displayed in the filtered/sorted manner

**Scenario 3: Select Stocks to Sell**
</br>&emsp;Given the list of stocks in a user’s portfolio
</br>&emsp;When the user selects one of the stocks
</br>&emsp;Then they will be able to enter a quantity to sell

**Scenario 4: Sell Stocks**
</br>&emsp;Given a user has selected a stock to sell
</br>&emsp;&emsp;And the user has entered a quantity to sell
</br>&emsp;When the user clicks the Sell button
</br>&emsp;Then the system will remove the stocks from the user’s portfolio
</br>&emsp;&emsp;And the system will update the amount of funds in the user’s account

# 6. UML Use-Case
![Use-Case Diagram](https://github.com/tommyv7806/integralinvesting/assets/67933601/a6ccc2c9-dc22-4cc0-b58d-308955aeffe3)

# 7. Requirements
<table>
<caption>Project Requirements</caption>
    <th>ID</th>
    <th>Title</th>
    <th>Description</th>
    <tr>
        <td width="10%"">LOGIN.01</td>
	<td width="30%">Ability to register a new account</td>
	<td width="60%">The system will provide the ability for new users to register ands create a new account</td>
    </tr>
    <tr>
        <td width="10%"">LOGIN.02</td>
	<td width="30%">Ability to log in to the application</td>
	<td width="60%">The system will provide the ability for users to use a 3rd party authentication system to log in to their account</td>
    </tr>
    <tr>
        <td width="10%"">LOGIN.03</td>
	<td width="30%">Ability to log out of the application</td>
	<td width="60%">The system will provide the ability for users to log out of their account</td>
    </tr>
    <tr>
        <td width="10%"">LOGIN.04</td>
	<td width="30%">Require user to verify email address upon registration</td>
	<td width="60%">The system will require the user to confirm their email address before being able to fully use the application</td>
    </tr>
    <tr>
        <td width="10%"">LOGIN.05</td>
	<td width="30%">Password reset functionality</td>
	<td width="60%">The system will provide the ability for users to reset their password.</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.01</td>
	<td width="30%">Provide a User Account page for managing user-related data</td>
	<td width="60%">The system will provide a User Account page where users can manage their data</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.02</td>
	<td width="30%">Ability to link financial institution to account</td>
	<td width="60%">The system will allow users to link their account with their financial institution</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.03</td>
	<td width="30%">Ability to add funds to account</td>
	<td width="60%">The system will allow users to add funds to their account from their linked financial institution</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.04</td>
	<td width="30%">Ability to deposit funds from account to financial institution</td>
	<td width="60%">The system will allow users to deposit funds from their account into their financial institution</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.05</td>
	<td width="30%">Ability to unlink bank account</td>
	<td width="60%">The system will allow users to unlink bank accounts from their application account</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.06</td>
	<td width="30%">Ability to edit user-related data</td>
	<td width="60%">The system allow users to edit their acount email address, username, and password on the User Account page.</td>
    </tr>
    <tr>
        <td width="10%"">INVST.01</td>
	<td width="30%">Create Browse page for purchasing stocks</td>
	<td width="60%">The system will allow provide a Browse page where users will be able to search for and purchase shares of different stocks</td>
    </tr>	
    <tr>
        <td width="10%"">INVST.02</td>
	<td width="30%">Ability to search for available stocks</td>
	<td width="60%">The system will contain a search bar where users can search for available stocks</td>
    </tr>
    <tr>
        <td width="10%"">INVST.03</td>
	<td width="30%">Ability to refine stock search</td>
	<td width="60%">The system will allow users to refine their stock search with sorting and filters</td>
    </tr>
    <tr>
        <td width="10%"">INVST.04</td>
	<td width="30%">Ability to purchase stocks</td>
	<td width="60%">The system will allow users to purchase available stocks if they have enough funds in their account</td>
    </tr>
    <tr>
        <td width="10%"">INVST.05</td>
	<td width="30%">Display number of shares that user owns on Browse page</td>
	<td width="60%">The system will display the number of shares that a user owns for a particular stock on the Browse page</td>
    </tr>
    <tr>
        <td width="10%"">PRTFL.01</td>
	<td width="30%">Create a Portfolio page to display a user's portfolio</td>
	<td width="60%">Add a Portfolio page to the application where users can maintain their portfolio data, such as selling shares</td>
    </tr>
    <tr>
        <td width="10%"">PRTFL.02</td>
	<td width="30%">Ability to view portfolio stock shares</td>
	<td width="60%">The system will allow users to view a list of their current stock investments</td>
    </tr>
    <tr>
        <td width="10%"">PRTFL.03</td>
	<td width="30%">Ability to filter and sort portfolio</td>
	<td width="60%">The system will provide users with the ability to filter and sort their stock investments in their portfolio</td>
    </tr>
    <tr>
        <td width="10%"">PRTFL.04</td>
	<td width="30%">Ability to sell stocks from portfolio</td>
	<td width="60%">The system will give users the ability to sell stocks from their portfolio based on the latest pricing information</td>
    </tr>
    <tr>
        <td width="10%"">API.01</td>
	<td width="30%">Configure free stock market API </td>
	<td width="60%">Add necessary connection information so that the application can interface with the stock market API.</td>
    </tr>
    <tr>
        <td width="10%"">API.02</td>
	<td width="30%">Ability to sell stocks from portfolio</td>
	<td width="60%">The system will give users the ability to sell stocks from their portfolio based on the latest pricing information</td>
    </tr>
    <tr>
        <td width="10%"">API.03</td>
	<td width="30%">Ability to sell stocks from portfolio</td>
	<td width="60%">The system will give users the ability to sell stocks from their portfolio based on the latest pricing information</td>
    </tr>
</table>

# 8. Entity Relationship Diagram
![ERD](https://github.com/tommyv7806/integralinvesting/assets/67933601/02f1b4b8-37e8-4cbc-8ea3-8d1bc3155a1c)

# 9. Class Diagrams
![UML](https://github.com/tommyv7806/integralinvesting/assets/67933601/1240b9e1-82bc-45c9-b340-1be4905615c1)

# 10. Data Access Layer
A data access API was created for accessing data from the different models in the application. This API is hosted using Azure. 

[Four class models](https://github.com/tommyv7806/integralinvesting/tree/main/DataAccessAPI/DataAccessAPI/Models) were created so far for the application:
1. UserAccount
2. BankAccount
3. Portfolio
4. PortfolioStock

Sample Code from one of the class models:
![PortfolioStockClassModelpng](https://github.com/tommyv7806/integralinvesting/assets/67933601/c44a35c7-f2c6-47fb-83e1-714ffa30b418)

HTTP Respopnse for the above class model:
![API_HTTP_Response](https://github.com/tommyv7806/integralinvesting/assets/67933601/65bea34d-9851-4525-ba11-a9d786e3f519)

Each data model was set up with some mock data. In order to see the API in action, please use the following links:
- UserAccount Data: https://dataaccessapi1.azurewebsites.net/UserAccount
- BankAccount Data: https://dataaccessapi1.azurewebsites.net/BankAccount
- Portfolio Data: https://dataaccessapi1.azurewebsites.net/Portfolio
- PortfolioStock Data: https://dataaccessapi1.azurewebsites.net/PortfolioStock
