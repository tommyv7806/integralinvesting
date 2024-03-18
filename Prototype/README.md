## Intro
This read me will walk users through the User Requirements for the application. 
   - Each section will contain a screenshot and link from the prototyped HTML pages. There is also some basic JavaScript an CSS styling included in the Prototype folder of the repo to add some basic functionality and styling to the HTML pages.
   - Each section will also contain the requirements that pertain to that page.
   - An explanation for each requirement is also provided. Please refer to the screenshot when reviewing the requirement explanations for each section.

## Account Related Requirements
[Link](https://github.com/tommyv7806/integralinvesting/blob/main/Prototype/Pages/AccountPage.html) to prototype HTML for this page 
![AccountPagePrototype](https://github.com/tommyv7806/integralinvesting/assets/67933601/9af54751-d112-42ea-af9d-a639c04c4e3b)

<table>
    <th>ID</th>
    <th>Title</th>
    <th>Description</th>
    <tr>
        <td width="10%"">ACCNT.01</td>
	<td width="30%">Ability to link financial institution to account</td>
	<td width="60%">The system will allow users to link their account with their financial institution</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.02</td>
	<td width="30%">Ability to add funds to account</td>
	<td width="60%">The system will allow users to add funds to their account from their linked financial institution</td>
    </tr>
    <tr>
        <td width="10%"">ACCNT.03</td>
	<td width="30%">Ability to deposit funds from account to financial institution</td>
	<td width="60%">The system will allow users to deposit funds from their account into their financial institution</td>
    </tr>
</table>

1. **ACCNT.01:** When users click on the "Link New Bank Account" button, a form will open up where they will be prompted to enter a name for the linked account, the routing number, the account number, and a confirmation for the account number. Once they submit this form, a new row will be added to the Linked Bank Accounts table.
2. **ACCNT.02:** Each row in the Linked Bank Accounts table corresponds to a linked bank account. Each row will also contain a "Withdraw Funds" to allow users to withdraw funds from their bank account and add it to their application account's funds. When the user clicks the "Withdraw Funds" button, a form will open where they will be prompted to enter the amount that they want to withdraw. 
3. **ACCNT.03:** When users click on the "Deposit Funds" button for a given row in the table, a form will open where they will be prompted to enter the amount that they wish to deposit from their applicaton account into their linked bank account.

## Portfolio Related Requirements
[Link](https://github.com/tommyv7806/integralinvesting/blob/main/Prototype/Pages/PortfolioPage.html) to prototype HTML for this page
![PortfolioPagePrototype](https://github.com/tommyv7806/integralinvesting/assets/67933601/562976e0-32b9-410e-9c35-17beb47b15a7)

<table>
    <th>ID</th>
    <th>Title</th>
    <th>Description</th>
    <tr>
        <td width="10%"">PRTFL.01</td>
	<td width="30%">Ability to view portfolio</td>
	<td width="60%">The system will allow users to view a list of their current stock investments</td>
    </tr>
    <tr>
        <td width="10%"">PRTFL.02</td>
	<td width="30%">Ability to filter and sort portfolio</td>
	<td width="60%">The system will provide users with the ability to filter and sort their stock investments in their portfolio</td>
    </tr>
    <tr>
        <td width="10%"">PRTFL.03</td>
	<td width="30%">Ability to sell stocks from portfolio</td>
	<td width="60%">The system will give users the ability to sell stocks from their portfolio based on the latest pricing information</td>
    </tr>
</table>

1. **PRTFL.01:** In the above screenshot, the right side of the Portfolio page is where the list of stocks that the user owns will be displayed. 
2. **PRTFL.02:** The "Sort by" dropdown above the table that displays the user's stocks will allow the user to sort their stocks by the different table headers. 
3. **PRTFL.03:** Each row in the table that displays the user's stocks has a "Sell" button. When this button is clicked, a form will open where the user will be able to enter the amount of shares they wish to sell for the selected stock. They will only be able to sell as many number of shares as they own.

## Purchasing Related Requirements
[Link](https://github.com/tommyv7806/integralinvesting/blob/main/Prototype/Pages/BrowsePage.html) to prototype HTML for this page
![BrowsePagePrototype](https://github.com/tommyv7806/integralinvesting/assets/67933601/c29224a3-edd8-474f-9a4d-24e35e783403)

<table>
<caption>Project Requirements</caption>
    <th>ID</th>
    <th>Title</th>
    <th>Description</th>
    <tr>
        <td width="10%"">INVST.01</td>
	<td width="30%">Ability to view list of available stocks</td>
	<td width="60%">The system will display a list of available stocks to the user with the latest pricing information</td>
    </tr>
    <tr>
        <td width="10%"">INVST.02</td>
	<td width="30%">Ability to refine stock search</td>
	<td width="60%">The system will allow users to refine their stock search with sorting and filters</td>
    </tr>
    <tr>
        <td width="10%"">INVST.03</td>
	<td width="30%">Ability to purchase stocks</td>
	<td width="60%">The system will allow users to purchase available stocks if they have enough funds in their account</td>
    </tr>
</table>

1. **INVST.01:** The Browse page is where the user will be able to search for new stocks that they might want to purchase. The latest prices will be displayed for the stocks on this page.
2. **INVST.02:** Using the search bar at the top of the page, users will be able to filter the list of buyable stocks. They can also use the "Sort by" dropdown to sort the results by any of the table headers' values.
3. **INVST.03:** Each row in the table has a "Buy" button. When the user clicks this button, a form will open which will allow the user to enter the number of shares they would like to purchase for the selected stock. 

