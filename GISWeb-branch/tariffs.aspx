<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tariffs.aspx.cs" Inherits="GISWeb.tariffs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <style type="text/css">
                body {
                    background-color: white;
                    font-family: verdana;
                    font-size: small;
                }

                .header {
                    background-color: #E62E24;
                    color: white;
                    font-family: verdana;
                    font-size: small;
                }

                .header2 {
                    background-color: #778CA1;
                    color: white;
                    font-family: verdana;
                    font-size: small;
                }

                .leftColumn {
                    background-color: #D2D7DD;
                    color: #656F7B;
                    font-family: verdana;
                    font-size: small;
                }

                .data {
                    background-color: #white;
                    font-family: verdana;
                    font-size: small;
                }

                .termsAndConditions {
                    font-size: small;
                }

                .termsAndConditionsStrong {
                    font-size: small;
                    font-weight: bold;
                }
            </style>
            <h1>Keypad Tariff from 1st March 2021</h1>


            <table border="0" cellspacing="3" cellpadding="3">
                <tr>
                    <td colspan="3" class="header">Click Energy Domestic Keypad Tariff Table</td>
                    <td class="header">Pence Per
                        <br />
                        KwH</td>
                    <td class="header">Pence Per
                        <br />
                        KwH</td>
                    <td class="header">Pence Per 
                        <br />
                        Day</td>
                    <td class="header">Pence Per
                        <br />
                        Day</td>
                </tr>


                <tr>
                    <td class="header2">Tariff Type</th>
                        <td class="header2">Timeslot</th>
                            <td class="header2">Additional Key Terms</th>
                                <td class="header2">Unit Price (Ex. VAT)</th>
                                    <td class="header2">Unit Price (Inc. VAT)</th>
                                        <td class="header2">Standing Charge (Ex. VAT)</th>
                                            <td class="header2">
                    Standing Charge (Inc. VAT)</th>
                </tr>

                <tr>
                    <td class="leftColumn">Keypad 24-Hour Standard</td>
                    <td class="leftColumn">24Hr</td>
                    <td class="data">No Fixed Term Contract</td>
                    <td class="data">17.13p</td>
                    <td class="data">17.99p</td>
                    <td class="data">10.608p</td>
                    <td class="data">11.138p</td>
                </tr>

                <tr>
                    <td class="leftColumn">Keypad 24-Hour 1 Year Discount (8.35%)</td>
                    <td class="leftColumn">24Hr</td>
                    <td class="data">1 Year Fixed Term<br />
                        1 Year Fixed Discount off Standard Rate<br />
                        £30 Early Exit Fee
                    </td>
                    <td class="data">15.70p</td>
                    <td class="data">16.49p</td>
                    <td class="data">10.608p</td>
                    <td class="data">11.138p</td>
                </tr>

                <tr>
                    <td class="leftColumn">Keypad Economy 7</td>
                    <td class="leftColumn">Day
                        <br />
                        Night 
                        <br />
                        Heat 
                        <br />
                    </td>

                    <td class="data">No Fixed Term Contract</td>
                    <td class="data">16.94p<br />
                        8.25p<br />
                        8.25p<br />
                    </td>
                    <td class="data">17.79p<br />
                        8.66p<br />
                        8.66p<br />
                    </td>
                    <td class="data">10.608p</td>
                    <td class="data">11.138p</td>
                </tr>

                <tr>
                    <td class="leftColumn">Keypad Powershift</td>
                    <td class="leftColumn">Peak<br />
                        Normal<br />
                        Low<br />
                    </td>
                    <td class="data">No Fixed Term Contract</td>
                    <td class="data">17.13p<br />
                        17.13p<br />
                        17.13p<br />
                    </td>
                    <td class="data">17.99p<br />
                        17.99p<br />
                        17.99p<br />
                    </td>
                    <td class="data">10.608p</td>
                    <td class="data">11.138p</td>
                </tr>
            </table>
            <span class="termsAndConditionsStrong">Keypad Economy 7 Tariff</span>
            <ul class="termsAndConditions">
                <li><strong>Day rate:</strong> 8am to 1am in winter and 9am to 2am in summer</li>
                <li><strong>Night rate:</strong> 1am to 8am in winter and 2am to 9am in summer</li>
                <li><strong>Heat rate:</strong> 1am to 8am in winter and 2am to 9am in summer</li>
            </ul>

            <span class="termsAndConditionsStrong">Keypad Powershift Tariff</span>
            <ul class="termsAndConditions">
                <li><strong>Peak rate:</strong> 4pm to 7pm on weekdays only</li>
                <li><strong>Normal:</strong> 8am to 4pm and 7pm to midnight on weekdays, and 8am to 7pm on weekends.</li>
                <li><strong>Low Rate: </strong>Midnight to 8am every day and 7pm to midnight on weekends</li>
            </ul>


            <h1>Bill Pay Tariff from 1st March 2021</h1>
            <table border="0" cellspacing="3" cellpadding="3">
                <tr>
                    <td colspan="3" class="header">Click Energy Domestic Bill Pay Tariff Table</td>
                    <td class="header">Pence Per
                        <br />
                        KwH</td>
                    <td class="header">Pence Per
                        <br />
                        KwH</td>
                    <td class="header">Pence Per 
                        <br />
                        Day</td>
                    <td class="header">Pence Per
                        <br />
                        Day</td>
                </tr>


                <tr>
                    <td class="header2">Tariff Type</th>
            <td class="header2">Timeslot</th>
            <td class="header2">Additional Key Terms</th>
            <td class="header2">Unit Price (Ex. VAT)</th>
            <td class="header2">Unit Price (Inc. VAT)</th>
            <td class="header2">Standing Charge (Ex. VAT)</th>
            <td class="header2">
                    Standing Charge (Inc. VAT)</th>
                </tr>

                <tr>
                    <td class="leftColumn">Bill Pay 24-Hour Standard</td>
                    <td class="leftColumn">24Hr</td>
                    <td class="data">No Fixed Term Contract</td>
                    <td class="data">17.13p</td>
                    <td class="data">17.99p</td>
                    <td class="data">8.471p</td>
                    <td class="data">8.895p</td>
                </tr>

                <tr>
                    <td class="leftColumn">Bill Pay 24-Hour
                        <br />
                        1 Year Discount (8.35%)</td>
                    <td class="leftColumn">24Hr</td>
                    <td class="data">1 Year Fixed Term<br />
                        1 Year Fixed Discount off Standard Rate<br />
                        £30 Early Exit Fee
                    </td>
                    <td class="data">15.70p</td>
                    <td class="data">16.49p</td>
                    <td class="data">8.471p</td>
                    <td class="data">8.895p</td>
                </tr>

                <tr>
                    <td class="leftColumn">Bill Pay Day &amp; Night</td>
                    <td class="leftColumn">Day<br />
                        Night</td>

                    <td class="data">No fixed Term Contract</td>
                    <td class="data">16.49p<br />
                        8.25p</td>
                    <td class="data">17.79p<br />
                        8.66p</td>
                    <td class="data">8.471p</td>
                    <td class="data">8.895p</td>
                </tr>

                <tr>
                    <td class="leftColumn">Bill Pay Eco Dreamer</td>
                    <td class="leftColumn">Day<br />
                        Night</td>

                    <td class="data">1 Year Fixed Term<br />
                        1 Year Fixed Price<br />
                        £30 Early Exit Fee
                    </td>
                    <td class="data">16.18p<br />
                        7.47p</td>
                    <td class="data">16.99p<br />
                        7.84p</td>
                    <td class="data">11.095p</td>
                    <td class="data">11.650p</td>
                </tr>

                <tr>
                    <td class="leftColumn">Bill Pay Economy 7</td>
                    <td class="leftColumn">Day 
               
                <br />
                        Night  
               
                <br />
                        Heat 
                        <br />
                    </td>

                    <td class="data">No Fixed Term Contract</td>
                    <td class="data">16.94p<br />
                        8.25p<br />
                        8.25p<br />
                    </td>
                    <td class="data">17.79p<br />
                        8.66p<br />
                        8.66p<br />
                    </td>
                    <td class="data">8.471p</td>
                    <td class="data">8.895p</td>
                </tr>

            </table>
            <span class="termsAndConditionsStrong">Bill Pay Day &amp; Night /Bill Pay Eco Dreamer Tariff</span>
            <ul class="termsAndConditions">
                <li><strong>Day rate:</strong> 8am to 1am in winter and 9am to 2am in summer</li>
                <li><strong>Night rate:</strong> 1am to 8am in winter and 2am to 9am in summer</li>
            </ul>


            <span class="termsAndConditionsStrong">Bill Pay Economy 7</span>
            <ul class="termsAndConditions">
                <li><strong>Day rate:</strong> 8pm to 1am in winter and 9am to 2am in summer</li>
                <li><strong>Night rate:</strong> 1am to 8pm in winter and 1am to 9am in summer</li>
                <li><strong>Heat Rate:</strong> 1am to 8pm in winter and 1am to 9am in summer</li>
            </ul>




        </div>
    </form>
</body>
</html>
