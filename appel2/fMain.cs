﻿using AxWMPLib;
using FarsiLibrary.Win;
using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Automation;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Models;
using YYProject.RichEdit;

namespace appel
{
    public class fMain : Form, IFORM
    {
        string test_url = string.Empty;
        string test_URL_CONTIANS = string.Empty;
        //string test_url = "http://vietjack.com/ngu-phap-tieng-anh/index.jsp";
        //string test_URL_CONTIANS = "http://vietjack.com/ngu-phap-tieng-anh/|.jsp";

        #region [ VARIABLE ] 

        readonly Font font_Title = new Font("Arial", 11f, FontStyle.Regular);
        readonly Font font_TextView = new Font("Courier New", 11f, FontStyle.Regular);
        readonly Font font_LogView = new Font("Courier New", 9f, FontStyle.Regular);

        IconButton btn_exit;
        IconButton btn_mini;

        Label lbl_title;


        long m_media_current_id = 0;
        MEDIA_TAB m_media_current_tab = MEDIA_TAB.TAB_STORE;
        string m_media_current_title = string.Empty;

        //Label m_msg_api;

        StringBuilder log;
        const int m_text_search_width = 99;


        #endregion

        void f_main_Shown()
        {
            this.Text = "English";
            m_search_Message.Text = string.Empty;
            m_media.Visible = false;
            lbl_title.Width = app.m_app_width * 2;// - (m_media_width + lbl_title.Location.X + 15);

            btn_exit.Location = new Point(3, 0);
            btn_exit.BringToFront();

            btn_mini.Location = new Point(25, 0);
            btn_mini.BringToFront();

            //m_media.uiMode = "mini";
            m_media.Width = m_media_width;
            m_media.Height = 44;
            m_media.Location = new Point(this.Width - (m_media_width - 2), 1);
            m_media.settings.volume = 100;
            m_media.enableContextMenu = false;
            m_media.BringToFront();

            //lbl_hide_border_left.BackColor = Color.Orange;
            lbl_hide_border_left.Height = 45;
            lbl_hide_border_left.Width = 12;
            lbl_hide_border_left.Location = new Point((this.Width - (m_media_width - 2)) - 10, -1);
            lbl_hide_border_left.BringToFront();

            btn_play.Location = new Point(this.Width - m_media_width, 3);
            btn_play.BringToFront();

            wd_media.settings.volume = 100;
            wd_media.enableContextMenu = false;

            m_search_Input.Focus();
        }

        public void for_test()
        {
            m_tab.SelectedItem = m_tab_Word;

            string content = string.Empty;

            #region

            content = @"well hello internet and welcome to my C   sharp tutorial you guys have asked for   it and I did my best to cover pretty   much the entire c-sharp language in this   one video in the description underneath   video you'll actually see links to all   the individual parts so not only will   you be able to jump to whatever you want   to learn about but you'll also be able   to see every single thing that's covered   in this video so I have a lot to do so   let's get into it   okay so if you are on Windows you no   doubt are probably going to use Visual   Studio 2015 or 2013 depending upon what   you want to use Community Edition and   you just go to Visual Studio calm or   just type in Visual Studio Community   Edition that's what I'm going to be   using in this tutorial however if you   are on Mac OS X or on Linux you can try   out the mono project comm right here and   download and give that a try everything   here is going to be able to compile on   any OS and now let's jump over and let's   start writing some code alright so here   I am inside of Windows and I'm just   going to come in here and open up a   Visual Studio 2015   sorry about the zooming some people   don't like that and here it is and it's   going to be real big here in a second   what I'm going to do is I'm going to say   that I want to start a new project and   specifically I want to start a console   application because that's gonna make it   easy for us all to see exactly what's   going on and focus on coding and click   OK and here we go this is exactly what   it's going to generate here on our   screen so let's just start writing some   code we're going to write all of our   code inside of static void main right   here and let's just start off with the   simple stuff comments just two slashes   there's your comment and then your   multi-line comment is forward slash star   and multi-line up above   and star and there you go so we got   comments out of the way now we're going   to be out putting everything onto the   console so in this situation I'm going   to say console dot if we type in right   there is not going to be a newline if we   type in right line there is going to be   a newline and we can just say a little   world and get the hello world thing out   of the way and then we can execute that   and you're going to see that there it is   a little world da-da-da-da-da opened up   in the screen don't worry about all this   stuff right here all right so let's   continue if you wanted to come in here   and get input from your user you could   say something like what is your name   like that and maybe you want to use just   right here because we don't want to put   a new line after that and then it's so   easy to get user input we're just going   to come in here well first we're going   to say that we want to store a string   that's how the input comes in and we're   going to store it in a variable name   name and just like everything else you   can use letters numbers underscores but   you should start off with a letter and   we're going to say console and read line   and that's how we're going to be able to   get input and then we'll be able to come   in here again and go console and write   line in this situation and then if we   want to output information in the screen   we could just go hello I'm going to put   a space inside of there and if we want   to combine two strings we can just type   in name with our variable name like that   now is how simple it is and we can   execute it what is your name and I can   say Dirk and it will say hello Derrick   instead let's get into data types now we   have boolean x' and a boolean is either   going to have the value of true or false   so can vote for example if we want to   store that inside of here we could say   true or false and that is a boolean   characters we could come in CH AR is   equal to dot a or whatever and this is   going to be able to hold a single 16-bit   Unicode character for us and the little   lines there is just there because I   saying hey you defined this variable but   you didn't do anything with it now let's   get into all of the different numeric   types you have integers with have a   maximum number of that big giant number   and you can actually get that number do   I just going int dot and then max value   with uppercase letters like that you   have   Long's which can have a larger non   floating-point number and then once   again you can get that value by just   going long dot max value and store it   over in this variable decimals are   actually massively huge and if you need   anything bigger than that you could go   and take a look at big integers and once   again you can get that value by just   going max value floats are going to have   very large numbers as well however   they're only going to have seven   decimals of precision that means if you   go in here and try to create a float   that is more than seven decimals that it   is going to cause errors and it's not   going to be accurate if you need   something even bigger go for doubles for   the most part you're going to use   doubles anywhere you can see how large   they are and they have fifteen decimals   of precision which is considerably   better there's a rundown of all the   major different data types you're going   to use being int Long's decimals floats   and doubles once again if we want to   come in here and combine our strings we   could do so with our console and write   line and just say something like max int   and all the code by the way is available   in the description as well and of course   it is all free there we go and you can   combine not only strings but you can   also combine strings with any other   different datatype this way and you can   see right there I printed that out on   screen for me another different data   type that is available to you is var and   basically the VAR data type is defined   whenever the program is compiled and   then its value isn't going to be able to   be changed so we could say something   like bar another name is equal to and   it's automatically going to say okay   this is a string because the data that   you store it inside of there is a string   however we wouldn't be able to come in   there thereafter and say another   and give it a value of two for example   that is going to cause an error so this   is a shorthand way for us to create a   variable based off of whatever we assign   to it however remember we're not going   to be able to change it to some other   data type because this is considered an   integer the thing that is interesting is   we can come in here and actually get the   type for our data type that we have here   so let's just go write line and we could   say something like another make sure you   put double quotes around this another   name is a and if you want to format or   throw in different pieces of data you   can just put inside here you're going to   start off with curly brackets and then 0   and then the next piece is going to be   squirrely brackets on one we'll get more   into that here in a second and then we   can come in and get the data type for   another name by just going another name   and then get type code and execute you   can see another name is a string so now   let's get into the different math   operations we can perform you can see a   whole bunch from right here we can add   subtract multiply divide this guy right   here is a modulus character which is   going to get us the remainder of this   division and you can see right here how   we can increment different values and   we're going to see exactly how they work   as well as decrement there's the   increment and decrement shorthands and   you can also see there are additional   shorthands this right here would be   equivalent to saying I is equal to I   plus 3 so this guy right here is just   the same as that this is just a   shorthand version of the same thing you   can see right here if we come in here   and add this is actually a remainder   operator so you can see it goes in there   and throws the decimal point in there as   well and you can see right here the way   that this is operating with this   shorthand notation see right here we   have at a value of 0 and you can see it   prints out a value of 0 so you might be   saying what's going on here basically   what it's doing is it's going and   getting the current value of I putting   that inside of there and then it's   performing the addition right here and   then when that's the same reason why it   jumps up to 2 is it whenever you put the   plus plus ahead of your variable like   you have right here it goes ahead and   performs the addition before transposes   it inside air and prints it   on the screen same exact thing happens   whenever we're decrementing and you can   also see exactly how this shorthand   notation works right there so that's how   you perform your basic arithmetic now   let's take a look at a casting and how   exactly that works now basically if no   magnitude is lost casting is going to   happen automatically but otherwise   you're going to have to set it up like   this so let's say we have double and pi   is equal to 3.
14 and we're going to   convert that into an integer you call   this integer pi is equal to you just put   little brackets inside of there and what   you want to cast it to and then the   thing that you want to cast and of   course make sure that you always end all   of your statements with semicolons that   is a requirement so pretty simple if you   want to cast from one to the other you   just go and put inside of there or   whatever you want to cast to as for math   functions that are built in there are a   whole bunch of them here or some of the   other ones and what the heck I might as   well just run through a couple of them   just so you can see exactly how they all   operate so let's go and create ourselves   two doubles number one and number two   and you can see here we can get the   absolute value with this function right   here we can get the ceiling which is   going to round up the value floor which   is going to round down max you're going   to send it to numbers it's going to give   you the maximum number or the largest   number men is going to do the opposite   you do the power you can do another   round and here is square root you can   see exactly how all of those operate   right here so absolute value 10.
5 could   see ceiling is going to round it up   floor is going to round it down max is   going to give you the maximum value man   is going to give you the minimum and you   can see the power operation and rounding   and square root right there so there are   some of the most common math functions   you use something else that we'd like to   do a lot is to generate random numbers   and let's just go and create a random is   equal to and you're just going to go new   random and then inside of here we can   do another console right and let's say   that we want to generate a random number   between 1 and 10 just be able to come in   here and then go Rand dot next and we're   going to if we went 1 to 10 we have to   go up to 11 right like this if we   execute that random number between 1 to   10 and you can see right there is the   value of 5 so there's a rundown of a   whole bunch of different things we can   do math related inside of Seashore now   let's go and take a look at some of our   conditionals   now the relational operators we're going   to have available or all the same year   very well used two less than greater   than greater than or equal to equal to   and not equal to and then with our   logical operators we're going to have   and or exclusive or and not I'm going to   show you exactly how they work so let's   just come in here and let's go over how   if statements work so let's say we have   int and an age and let's give it a value   of 17 can then come in and go if and   we're going to use two brackets here age   is greater than or equal to 5 and and   this is going to allow us to do two   comparisons at once age is less than or   equal to 7 then we're going to put in   our curly brackets and define what   happens if that comes back is true for   this to come back is true since we're   using and here this has to be true and   that has to be true we'll say something   like go to elementary school now   likewise we continue making comparisons   just by going else if and then we could   throw in another conditional inside of   this age greater than 7 and age less   than 13 in that situation once again we   could do another console right and   instead change this to middle school and   then finally as a default if you get   through all of those we can say else   console right and we could say go to   highschool and there you go that's   exactly how if-else F and else work as   well as the relational operators and the   logical operators when it's the example   of how the logical operator or works we   could also come in here   Kengo if age is less than 14 or age is   greater than 67 could come in here and   say something like you shouldn't work   that's not a political statement right   there that's just based off of   retirement ages and when it's legal to   work in the United States anyway all   right so there we go and you can see   that this is only going to come back   true or this is going to come back   through if either this is true or that   is true and of course we could use the   exclusive-or which is going to only come   back if one of them is false and one of   them is true but this was the one you're   going to use most of the time another   thing we can do is go console right and   just to demonstrate how not works not   true it's just going to take whatever   you have here and it is going to give   you the absolute opposite of it so we   can put not true for example right like   this and if we execute it you're going   to see not true is equal to false also   take a look at the ternary operator and   it's going to allow us to assign a value   based off of a condition so we'll put a   boolean in here but this could be   anything this could be an integer or   double or anything and we're going to   say if age is greater than or equal to   16 I'm going to put a question mark   inside of there then we'll put true and   then a colon and false basically what   this is saying is if this value or this   condition comes back true we want to   assign the value of true to can drive if   it comes back as false we're going to   assign false to it like I said we could   also come in here and do something like   integer and then assign some arbitrary   number that is how we would like to work   with this instead but that is how the   ternary operator works inside of c-sharp   another condition that we can use is the   switch statement yes we have switch   statements we could say age come inside   of here and we're going to be able to go   case 0 like this and then in the case   that age has a value of 0   we could go console right and we could   say infant and you're going to have to   put your break statements in here to   jump outside of there otherwise you're   going to get errors you're also going to   be able to come in and stack these up so   let's go case and then we could also   check case two and in that situation we   type toddler out on   so if the value of age is either 1 or 2   it's going to perform this operation   that's inside of there another thing we   could do is this isn't specific to   switch statements but you can use go   twos inside of Seashore do something   like go too cute and then we could jump   outside of this like this and then type   in cute the colon like that and this   would jump completely out of the switch   statement and we could say something   like toddlers are cute or something like   that that is how go-to works inside of   there you can see there you are there's   a label jumps to that hopefully you   almost never use that because go-to   statements can cause major nightmares   and then finally of course we're going   to be able to come in and find a default   situation if nothing else matches and in   that situation we could just say child   and then through our break statement in   there you go that is how switch   statements work if ternary operators as   well as go-to now let's jump over and   take a look at looping now with looping   we're going to continue executing   statements as long as a condition is   true so in this situation we're going to   create an integer here give it a value   of 0 because we're going to need a value   that is going to increment over and over   again and I'm just going to call this   into I just to keep this nice and simple   let's first talk about the while loop   and in this situation we're going to   continue looping as long as I is less   than 10   I could however come in here and say if   I is ever equal to 7 in that situation I   want to do something different I'm going   to increment the value oh I always have   to do that if you ever ever have any   sort of errors with your loops there   probably have something to do with not   incrementing when that situation what   we're going to say is continue so if I   is equal to 7 what we want to do is we   want to increment the value to 8 right   here I is now going to be equal to 8 and   then we're going to say we want to   continue with our loop which means we're   going to ignore everything that comes   out right here we're going to jump back   up to the loop and then continue working   with what we got right here we could   also come in and say if I is equal to No   mine in that situation we can use our   break statement again which is going to   jump us completely out of a loop no more   while no more check-in anything we are   all done and if I is not equal to seven   or nine we could do something like this   if we want to only be able to print out   odd numbers we could say if I remainder   of two is greater than zero well in that   situation we want to print out our value   of I and do it right like that and then   of course we want to make sure that we   increment I before we leave our while   loop so this is going to come in here   the reason why this is only going to   print out i if it is an odd value is   here if we would take like a four and   then do modulus of two that is not going   to give us a remainder so we're checking   for a remainder and the remainder would   only occur if it is in odd number and   that is the reason why only odd numbers   are going to print that situation if we   go and execute this you can see it   prints out one three and five right   there   now the other looping operation we're   going to take a look at we're going to   take a look at all of them but we're   also going to take a look at two while   and do-while basically going to be used   instead of Wow whenever you want to   guarantee that you are going to   definitely go through the code in the   loop at least once before you check if   you should continue cycling through the   code inside of the loop and this is   actually quite useful let's say that we   have a string that we have the name of   guess and we want to cycle through this   loop and consider or continue getting   input from the user   until they guess the proper number so we   can come in and go console and we can   say something like guess a number like   that we could then go and get the user   input and store it inside of guest by   going console read line just like we   covered previously and now what we're   going to do is the while part is   actually going to come after the do part   so you can see right here this code is   definitely going to be executed at least   one time because after we go through it   the first time we're then going to say   not guess equals and this is how we   check for equality with strings so we're   going to put 15 inside of there which is   a string and we're going to come   to the string that was input here by our   user so what we're saying is while gas   is not equal to 15 we want to continue   executing this loop over and over and   over again and this is how we check   string equality by the way is with the   string and then equal sorry like that we   can execute it and it's going to say   guess a number and we can start guessing   numbers until we get to 15 and then we   know that we got the right number   there's an example of do-while now let's   take a look at the for loop which is   basically going to allow us to put the   iterator and all of its initialization   all in one place so we're going to go   for we're going to find the datatype for   the guy that we're going to need to be   iterating over and let's say zero then   we're going to define exactly how long   or the condition in which we're going to   continue execution while I is less than   10 and then we're going to come in here   and we're going to say how much we're   going to increment each time and you can   of course increment this by 2 or 3 or 5   whatever you want to do doesn't matter   then we could say something like let's   say we want to do similar thing with   what we did before we could say if I   assign to greater than zero so we're   going to check so that we're only going   to print odds out on our screen again   and we could say console.
writeline right   like this and then put I inside of there   execute it and I wouldn't put I inside   of that would I do wrong I put I inside   of quotes no big deal got rid of that   execute it and we can see that it only   prints out our odd numbers so that's how   the for loop differs for it and we also   have another one which is known as the   for each loop and we're going to see   more of this later basically what it's   going to do is it's going to cycle   through every item in an array or a   collection and a string is actually a   collection of characters and to do this   let's go and get and create ourselves a   string and let's just call this random   string it's equal to and now we'll be   able to use the for each by just typing   in for each no space we're going to   define the data type that we're going to   be pulling from this is a string which   is just a bunch of individual characters   so we're going to be getting out each   individual character temporarily we're   going to store it inside of a variable   named C can be anything you don't this   variable name can be anything you put   whatever just don't put C har because   that's going to cause confusion and then   you type in this string that you want to   cycle through or the collection like I   said as the tutorial continues I'm going   to show you how to cycle through other   collections and then we could just come   in here and we could print out each one   of those individual characters inside of   the string out on our screen and there   you can see they are here are some   random characters you can also see it   puts out the spaces that are inside of   there because the spaces are also   considered characters okay so a whole   bunch of things let's go and take a look   at strings now one thing it's important   to note is if you're creating a string   and let's say you wanted to put a double   quote inside of that string how is it   going to know where the string ends well   we have things called escape sequences   if you wanted to put a single quote   inside of there you just go and put a   backslash and a single quote I'm going   to put a double quote put a backslash   and a double quote you want to put a   backslash because we're using back   slashes you have to put two backslashes   just to get one if you wanted to go   backwards back one space put a backslash   and 8b and there's a newline and there's   a tab there's a couple other ones but   these are the ones you're going to use   all of the time so let's go and create   ourselves a string and do some different   string operations   let's go sample string is equal to and   let's just a bunch of random words like   that make sure you put semicolon at the   end and we're going to be able to do all   sorts of different operations on this   we're going to print some stuff out on   the screen we can check if the string is   empty for example and this is something   you'll do with if statements a lot so   we'll say is empty and then we'll just   go and get string we want to use the   string class this is like a an object   we're going to get more into objects and   so forth as we go on this is just a nice   handy tool we're going to be able to use   and one of the functions it's going to   be inside of here that we'll be able to   use is is null or empty want to check   both so we'll say through our sample   string inside of there and then close   that off this guy right here is going to   allow us to be able to very easily   figure out if our string is null or   empty just   says we could also let's just leave this   is empty and we could say is null or   let's say we wanted to check if it only   contained whitespace well there you go   just type in is null or whitespace and   we'll say something like string length   want to get the total number of   characters inside of there in this   situation we're going to go sample   string like that   and dot and length with uppercase   letters and no brackets like that   execute it you're going to say no it is   an empty no it doesn't just contain   whitespace and the total string length   is 23 characters as you can see right   there let's go and do a couple other   different functions all bunch of   functions for Strings let's say let's go   console and let's say that we want to   search for bunch the word right there so   we could say something like index of   bunch now each of these individual   characters are going to be assigned   indexes like a raise or hopefully you're   aware of how arrays work and how indexes   work basically this is the zero index   this is the space here is the one index   two three four five six and I messed up   Bunch   so what I want to do is I want to find   out exactly where inside of this string   the word bunch actually is stored and if   it can't be found it's going to give me   a value of negative one so you'll also   be able to check that with your if   statements if you want to find it you   just type in index of pretty simple   pretty much exactly what you would think   it would be and then inside of here   you're going to type in the word that   you are specifically looking for and put   that inside of quotes and of course   inside of here instead of string we're   going to put in the string that we are   actually going to be searching for let's   go into a couple more of these just so   we can see them do another one let's say   that you wanted to get a substring like   you wanted to get we found bunch and we   wanted to be able to pull bunch out of   that string well we could do something   like just a second word again we're   going to be using sample string because   that's what we're going to be searching   through sample string and let's say that   we wanted to start at the second   character now first off we would type in   substring   a subpart of the total strength we want   to start this guy here is going to say   hey it said index - then you'll say ok   fine I want to grab it I want to start   at index two and reach the whole way out   to index six and then I want to store   that or print it in this situation and   this is substring it's a little area and   there you go if we execute that you're   going to see right here index of word   bunch is two and then we jumped in there   and we grabbed the word bunch and pulled   it out and print it out on a screen or   we could just store it   we could also let's create another   string it's called as sample string two   and we'll say something like more random   words do some comparisons between these   guys we could check if the strings are   equal strings equal again simple string   and to check equality this is very   important say equals like that and then   this will be sample string two that's   how we check equality could also do   string comparisons I just like to print   these out on the screen that's reason   why I'm using right line here we could   check if a string starts awhile with the   two words a bunch so you say something   like starts with a bunch right like that   and just to show you exactly how we   would put in the double quotes we could   do backslash n backslash and we could do   Sam string this starts with pretty easy   to remember these after you do it long   enough and code completion helps a lot   so we're going to check if that string   starts off with the words a bunch right   like that we could also likewise go and   figure out if it ends with certain   values so and with words words is what   I'm going to be searching for in this   situation and what again we'll say Sam   string is what we're going to be   searching for and I bet you can guess   what I'm going to type here ends with   like that and we could just submit and   say words and if we execute you can come   over here and you can see starts with   the two words a bunch comes back is true   ends with the words words you know ends   with words and that also goes back as   true and there we go you can also see   strings are not equal right so a couple   other different string operations we can   perform here we could come in here and   store restore value   new value inside of sample string will   also be able to come in here and go   sample string and let's say we wanted to   trim any white space for the string that   is either at the beginning or the end   you just do that with trim right like   that you could also do a similar thing   by just going trim and if you just   wanted to get the white space off the   end or trim start if you just wanted to   get the white space off of the beginning   and that's how trimming works we would   also be able to come in here and replace   words inside of the string let's say we   want to store them in sample string just   like we did before we would just go   sample string and we could say replace   and we could say what we wanted to   replace which is words and instead   replace it with characters that's how   easy that is to do and I could print   that out but I'm sure you know what that   looks like could also remove starting at   a defined index so we could say sample   string equal to and get sample string   and let's say we want to start removing   at index 0 which is the first very first   place up to index 2 doesn't take index 2   out so what we're going to be doing is   just removing 0 1 so bunch is going to   be the beginning of the string after we   perform that remove operation right   there and another thing we could do is   we can create string arrays an array is   just a whole bunch of strings all stored   under one name in this situation it's   going to be names arrays can store a   whole bunch of different values like I   said I'm sure if you're watching this   you've seen other programming languages   you know what an array is so let's say   in this situation this is how you create   an array by the way just type in   whatever the data type is for all the   values in the array 2 brackets right   there then we're going to give it a name   we're going to say new string and we're   going to define exactly how many blocks   we want to set aside and then we're   going to inside of this store a whole   bunch of different values so let's say   neighbors I have or something like that   there you go just created that we're   then going to go into the console and   show you how to join all of these into a   string so we're going to convert this   array to string array into a string all   on its own so we can say   list we're going to be using the string   here and we're going to say join like   that join the string array into one big   giant just array then inside of this we   have to define exactly how we want those   to be separated so I want to calm in a   space and then I'm going to find the   array name that I want right there   and there you can say aims and then   we'll have name list and that prints out   all of those difference guys right there   on our screen another thing that's   pretty important is what we would use   for formatting basically I'm just going   to show you a whole bunch so we'll say   string this is call this format string   it's equal to if you want to format a   string you just go string format and   let's define a whole bunch of things so   I said if you want to transpose or move   different values inside of here I'm   going to use curly brackets and in this   situation let's say that I want the data   that's going inside of here to be a   currency so it's going to have a dollar   sign inside of it well pretty easy just   go like this and then outside of that   we're going to put a comma and 56 that's   automatically going to convert it into a   currency since I have the little 0 and   the C inside of there we could also   define decimal places with this value so   it's like this in this situation we're   going to say that we require well   actually the 1 is going to define that I   want the second value right here in the   comma that's what that means some people   get confused by that for some reason   then what I'm going to do is I'm going   to say I demand that there are at least   two zeros and then two additional zeros   for the decimal places that's going to   handle decimal places for me here I'm   going to throw in another one and let's   just say that the value that I want to   put inside of there in that situation is   going to be 15 point five six seven so   it's saying hey I demand there's at   least two numbers inside of there and   then two decimal places so what's that   going to do it's going to chop off the   seven and inside of here we could also   define decimal places and if we put a   hash symbol inside of here what that   means is we don't care if you put a zero   inside of there or not it's basically   like I will   you know whatever put a zero inside   there or do not put a zero inside of   there and that would work if we had a   value that looked something like point   five six so in that situation it's going   to say well there's no zero here so   we're not going to put a zero here but   we demand that there's at least two   decimal places so that's what that means   and then we could also define that we   want the thousand separator to come in   here for our third or a fourth number in   this situation and we would just say   zero and then we put a comma and then   another zero and close that out and   whenever you see this it will all make   sense and we could say a 1000 right like   this and then we want to print this out   on our screen and you can see exactly   how it printed all those out in the   thousands and let it out it Ida da da da   there we go there's a whole bunch about   strings now let's take a look at the   string builder now each time you create   a string you're actually creating   another string in memory string builders   on the other hand are used whenever you   want to be able to edit a string without   creating new ones and it's easy to make   one you just go string builder and let's   just call this SB and we're going to say   new string builder that's what we're   going to be visiting a stream builder   object and then be getting more into   objects in a second and if you want to   be able to append another string to the   end of your string builder you can use   append line if you wanted to have a new   line at the end of this but if you don't   want a new line you could say this is   the first sentence and then you could   continue to append you could also do   append   format so pen format say my name is and   then inside of curly brackets the first   thing you want to put inside of there   and I live in and then you can put the   next thing you want to put inside of   there and then you can put in whatever   you want to transpose inside of there   there's three different ways remember a   pen line we put a new line after this if   you do not put a pen line then it's not   going to put a new line there   you could also come in and if you   decided you wanted to delete your string   builder all the information inside of   there just call clear and then you could   also let's get rid of that you could   also go and call replace if you want to   replace every instance of the very first   thing you type in with the second thing   so let's say we want to replace the   letter A and   the letter e everywhere inside of our   stringbuilder which is going to be a   culmination of both of these strings   that we created right here you could   also remove characters starting in an   index and then up to whatever you put   inside there by going remove so let's   say we want to start at index 5 and go   up to but not including index 7 and then   let's just output everything on the   screen see exactly what it looks like   and to do so you need to convert to the   stringbuilder into a string so we go to   string and execute it and you can see   right there this is the first sentence   my name remember we replaced all the A's   with ease is Derek and indebted edit I   and I live in Pennsylvania ok so there   you go there's string builders in a   rough overview of some of the things we   can do with string builders now let's   take a look at arrays now to declare an   array remember you have to put the   datatype of every single value inside of   that array and then we're going to give   it a value or a name or a variable name   and that is how easy that is to set up   that remember that is just declaring the   array not going to be able to do   anything with it if you also want to   come in and declare an array and then   also define how many items you want to   put inside of it you're only going to be   able to put the number of items inside   of an array as you define here so here I   have an integer array that I expect to   have 5 values in it and no more than   that and that's exactly how you have   that set up you could also come in and   declare and initialize an array all   right like this no and then here whoops   don't put new in that situation we're   going to say 1 2 3 4 5 bottle of Allah   and there you go and it's automatically   going to set aside enough space to   contain all of that data that you said   you want inside of your array pretty   easy to get your string length or your   array length array length like that and   say I want the second array right here   just type in length and there you go   that's going to get you the total number   of items you have right there of course   that would be good if you want to cycle   through all of your different data also   going to be able to come inside of here   and let's say that we want to get   item 0 inside of our array and just like   the strings the first item inside of   your array is going to be 0 index 1 2 3   & 4 if you want to go in there and grab   the very first item inside of there   you're going to go R and array 2 and   then inside the brackets put the 0 and   that's going to help put that on our   screen if we want to cycle through our   rice like I said before there's multiple   different ways to do it let's just do   this one like this we're going to start   off with 0 because that's the very first   item in the array I'm going to say as   long as I is less than random array 2   dot length   that's how length is useful length is   useful in a couple ways though and then   we can print out the information on our   screen and we could also print out the   index so let's say right line and then   we want to put first item inside of   there and then which is going to be our   index and the next one's going to be the   actual value that we have inside of   there and then we could say I and then   let's go and get the value out of our   array right like this and then put the   value of I inside of there because   that's what's going to be incrementing   as we're cycling through all this stuff   and another way we could do this of   course is with our for each block that   we have again we're going to define the   data type that we want num is going to   be the temporary holding cell for each   item in the array that we're cycling   through random array is going to be what   we're cycling through and then console   right and then we could just put inside   of here numb just to keep that nice and   simple let's execute just see all the   different things and you can see right   here array length is 5 what is in the   index of 0 1 you can see it's printing   out all the different things and for   each it's also printing out everything   right there we could also come in here   and say hey what's the index for a   specific item inside of my array so for   example we could say where is 1 so we'll   search the entire array for the value of   1 and it's going to kick back the index   and to be able to get our index for that   guy we're going to use an array object   index of and we're going to pass in the   array name that we're going to be   searching for and then the specific   thing that we are searching for inside   of that iraq like that and that's how   easy that is but also   here of course and create some string   arrays and let's just call this names   and I'm going to initialize it from the   start so it's just put in some random   names we could join an array into a   string just like we did before let's go   name string is equal to and then we're   going to call string join we're going to   say what do we want between each of   those strings what we want to call them   in a space and then the actual array   that we're going to be working with and   it's going to automatically join those   together and save them into a string we   could also come in and split a string   then into an array so string there's a   string array call this name array is   equal to and name string and then call   split on that and then inside of it   we're just going to put what separates   all those names in the strings which is   going to be a comma and execute that see   where is one it's in the zero index and   the other stuff then we'll actually do   anything but if you print this out   you'll see exactly how those work and   why don't we just come in here and just   create a multi-dimensional array just so   we can cover all that so let's say we   wanted to create an integer   multi-dimensional array let's just say   it's going to be a two-dimensional right   we're going to not put anything inside   of there but we are going to put a comma   mult array is equal to new and it's   going to be an integer multiple array or   multi-dimensional array and let's say we   want it to be 5 by 3 that's exactly how   we would define that we could also of   course come in and do pretty much the   same exact thing and initialize the   multi-dimensional array at the same   exact time so multi array 2 is equal to   this is sometimes easier because it's   easier to see how everything is going   inside here so there we go and this is   how we would separate each individual   part of our multi-dimensional Rex a 2 &   3 you see there's another column inside   of there and then inside this one we'll   put 4 & 5 and then finally to close that   off we've just put our semicolon inside   of there we could then cycle through our   multi-dimensional write in a couple   different ways we could either use for   each like this again the data type num   in multi our I to that guy right there   and then we could   on our screen just each one of those   individual noms which is where those   going to be stored or sometimes we also   want to be able to print it out in   different ways so let's say we want to   cycle and have access to all of the   different arrays we could also go int X   is equal to when we're starting off zero   index X is less n and we could say multi   ray to going to get the length of it we   could go get linked and throw in zero   inside of there that's going to give us   that and then we could say X plus plus   if you were wondering you could also go   X plus equals to one right like that but   let's just just trying to cover   everything as I'm going through here as   things pop into my head okay so that's   going to give us the first part of our   array but we want to get the second part   of our array as well so we're going to   go int and let's say Y is equal to zero   and we're going to cycle through this as   long as Y is less than the multi array   two and again we're going to go get   length like this and in this situation   I'm going to put one inside of there   then we're going to increment the value   of y as well and now we can access   everything so we could go   console.
writeline and we'll throw in the   first part we're going to put inside of   here throw like a or same inside of   there and we're going to get another   value inside of that and then the final   actual value that is stored inside of it   right like this now what we can do is   come in here and grab x and y and then   to actually get the value that is stored   in our multi-dimensional array there you   go Balto array two and then throw in x   and y at the same time and we could save   that and execute it and you're going to   say it prints out our multi-dimensional   array which is sorts often 0 0 index and   this is the value and so forth and so on   so there is a heck of a lot about a race   let's go and take a look at lists now a   list is like an array however unlike an   array whenever you add new items to a   list the list is going to automatically   resize for you and how we're going to   create a list as we're just going to   type lists and then the type of data   it's going to contain create a number   list and then you just go new list and   then the data type that you want to   and the brackets for the function now to   add items to our list we just type in ad   and you can just throw in as many   different items as you'd like to and   there you go and you don't have to worry   about the size of your list other things   kind of neat is it's very easy to add an   array to your list so let's just create   an array here and there it is now you   can just go num list and of course the   data type has to be the same so make   sure that you do that and then you're   just going to go add range instead and   then make sure you put a little dot   inside of there there we are and then   inside of here you just put in the name   of the array that you want to put inside   of there pretty simple you can clear   your list by just going on list and then   clear like that but why would you want   to do that can also copy an array into a   list so I'll just create a list let's go   to number list to new list and the data   type you want inside of there and then   in this situation you pass in the array   that you want to put inside of there and   you could also create a list with an   array in a very similar way so let's   list three equal to new lists and then   inside of it we'll just define our array   inside of here so it's an integer array   and then we could just put 1 2 3 & 4 or   whatever you want inside of there let's   say you wanted to insert an item into a   specific index   let's just go nom list say insert and   you're going to say first off the index   you want to insert it into so the one   index number it starts at 0 and there we   go we just inserted that we could then   remove an item specific to its name so   remove 5 you could remove added index so   remove a specific index with remove at   and then of course you could cycle   through your list just jump it in here   and just like you would with an array   and you can also see here if you want to   get the length of your list you use the   count function right there if you   execute you can see all of those lists   items print out right there also be able   to come in here and return the index for   a specific value pretty easily so say   something like 4   is in index and we're just assuming that   four is inside or here and we could say   number list let's just change this to   three just use a different one and then   we'll say index of like that and then   the specific value that you're looking   for right there and it's going to return   negative one if it doesn't find anything   likewise you could come in and check to   see if an item is inside of a list so we   could just come in here again and we   could say something like five in list   we'll just go number list again and   contains and then the item that we're   looking for specifically inside of there   and you could also search for a value in   a string list so let's create a string   list same exact thing like this new and   we'll go and create a new string array   right inside of here and we could just   put in some names and then close that   off and then we could say something like   Tom in list and then we'll use the   string list here instead so string list   and you're going to do pretty much the   same thing contains another thing that's   useful is we could actually use   lowercase letters here however if you do   that you're going to want to go string   compare that guy right there and then   ordinal ignore case right there so what   we're going to be able to do is search   for Tom and even though this is a   lowercase T it's going to be able to   match that right there and you want to   close that off then the final thing we   could do also is if we wanted to sort   our list we could go string list and   sort and it's going to automatically go   in and sort that alphabetically or   numerically depending upon the type of   list items you have all right a whole   bunch of stuff let's go and take a look   at exception handling next now there are   countless numbers of different   exceptions I'm just going to focus in on   exactly how exception handling works   inside of c-sharp here and on on my   website or in the link in the   description you're going to find a link   to all of the different exceptions I'm   going to do a / 0 exception right here   and we use exception handling anytime we   want to keep errors from going out to   the user of our software so let's go in   is equal to int and if we want to   convert from a string into an integer we   use parse or int dot parse or double dot   parse or whatever how we can do our   conversions because we want to convert   the read line which is a string into an   integer and then we can print some stuff   out here on the screen something like 10   divided by and then we'll throw in   whatever they put inside of there is   equal to and then we'll throw in our   final result and then we'll just put in   num and then we'll put in 10 divided by   num that's the second thing that's going   to show up there on our screen   all right so inside the try block we   have what we think is going to maybe   lead to an error which is division by   zero error now what we have to do is   catch an error and we do that with catch   like you probably would have guessed and   this one's called divide by zero   exception and I'm going to put e^x   inside of there so I can get additional   information about that exception console   right and we could just do something   like we can define now exactly the error   that the user is going to see so that   they don't just get a whole bunch of   junk all over their screen so we're   going to say can't divide by zero custom   error that we or a custom message that   we will show them if there's an error   now with the e^x we're going to be able   to come in here and get additional   information on the exception so two   different things we can get we just go   e^x   and then we say get type and then we can   say name and that's going to get us the   name of the exception that was triggered   and then we could also come in and get   an error message that describes the   exception just like that another thing   we could do is go throw e^x which is   going to throw the exception for another   part on another catch block to be able   to handle and another thing you could do   is throw a specific like let's say we   catch an exception here and we want to   throw another exception we could go   throw new and in valid operation   exception just whatever that is you know   just to throw something out there and   then we could also put a message inside   of here operation failed and also along   with that throw the exception   information inside of there so we'll be   able to handle that put the semicolon on   the outside of course   and then finally we also would have our   default option for caching every   exception that isn't caught anywhere   else and that's just catch exception   with the generic exception port there   and then we could put all types of error   messages we could just come in here and   grab this right now and throw that in   there so there's the basics of exception   handling just one to cover exactly how   that operates and now I think it's   definitely time for us to talk about   classes and objects all right so I'm   guessing you guys know the difference   between a class and an object basically   a class is just a blueprint we're going   to need to use to define the attributes   and capabilities of the objects we're   trying to model okay so let's just come   in here and create I'm going to change   the class name here to animal and I'm   going to come in here and define some   properties for this guy now there's   going to be different ways we can limit   access to the different properties we   have we're going to have public which   means that access is basically not   limited protected is going to say that   access is limited to specific class   methods so the methods inside of our   class however it's also going to be   accessible to subclass methods that we   create later on and private is going to   say basically that access is limited   only to this specific classes methods so   we're just going to keep this simple and   create some public fields or attributes   or whatever you want to call them and   see short provides a really easy way for   us to set getters and setters we just do   that it automatically generates them so   we're going to have our animal it's   going to have a height and we're going   to be able to set those using getters   and setters methods and we'll also have   weight and you're going to see what   those look like here in a second also   for each animal that I'm going to create   it's going to have a string and it's   going to have a name or why don't we go   and have this beast sound specific sound   it's going to create then I'll show you   how to also come in and create your own   getters and setters if you'd like to do   that and let's say we have string like   that and then name and since we're going   to create our own getters and setters   we'll get rid of that well now what   we're going to be able to do is go   public string and I'm going to make this   an uppercase name unlike that and then   inside of this create the getters and   setters and the get   that I want to use here is return name   and the setter that I want to have here   is name is equal to value you may say to   yourself well why would you want to do   that values very important value always   has to be used because that's the   default for setting any of these   different items in c-sharp so make sure   you use that this you can have it be   anything but and in regards to why are   we using getters or setters basically   with the getters and setters we're going   to be able to either limit access with   the get port or only allow them to set   certain values with the set part so for   example we could come in here and maybe   check to make sure that the name that   they want to set for our name property   doesn't contain numbers if it does we   send an error message it says hey you   need to give me another name it doesn't   have numbers in it otherwise I'm not   going to allow you to set the name and   so forth and so on I figure you got the   handle on that what we're going to need   also now that we have some attributes   for our animal object is we're going to   need a way to initialize these new   animal objects and to do that you're   going to use a constructor now the   default constructor is automatically   going to be created for you unless you   create one on your own then it's not   going to be created how you would create   a default constructor that doesn't   receive any attributes is like this then   what you're going to be able to do is   come in here and set values now whenever   you create an animal object later on   you're not going to have any way of   knowing what the name is so if you want   to refer to attributes for any type of   animal object you use this followed by   the attribute you want to set and in   this situation I'm just going to set a   whole bunch of default values for every   animal object that's created that isn't   passed attributes you're going to see   here in a second what it looks like   whenever attributes are passed in now   we're going to define the weight and   we're also going to come in here and   this is also how you set values with a   dot operator like that and then what's   also set a sound and of course we're   going to have to have the name I'm going   to put no name inside of there just so   you can see very easily that there's no   name assigned and no sound and then   let's say that we also want to come in   here and maybe track the number of total   animal objects have created well how   could we do that well and one thing to   think about is this is going to bring us   to the concept of   statics well before I get into that let   me jump in here and just show you how to   create the other constructor that's   going to set our variables for us or our   attributes again you're going to go   public whatever the class name is and   then you're going to find the data types   for everything that's coming inside or   here and sound and then you're basically   going to do exactly the same thing we   did here we're just going to go this   animal is going to have these values   assigns to its specific attributes so in   this situation we'll say height and here   we'll say weight and then here we'll say   whatever the name is they passed inside   of it and then this is going to be   whatever the sound is they passed inside   of it so you can see here where it makes   a lot of sense that these animals would   have Heights and weights and names and   sounds okay however I also want to be   able to track the total number of   different animal objects have created   now does it make sense for an animal to   be able to track or count the number of   animal objects created now in situations   in which you get that answer whenever   you want to add a capability but it   doesn't make sense for the object to   have that capability that is whenever   you're going to use what are called   static fields now static field is going   to have a value it's going to be shared   by every object of the animal class in   this situation and like I said we're   going to call something static when it   doesn't make sense for the actual object   to be able to perform a certain   operation so a static field attribute is   starts off a static data type num of   animals okay so that's what you want to   store inside of here and like I said   this values going to be shared by every   animal object ever created ever now you   may also want to be able to get access   to this static value so we're also going   to create inside of here a static method   and we're going to make it public so   anybody can get ahold of it not just   class methods it's going to return an   integer and get of animals no attributes   going to be passed inside of this   function it's just going to return on   all the animals whatever it's called and   another thing we can do here is each   time one of these constructors is called   we're going to be able to come in and go   numb all animals right like this paste   this inside air and   increment that and then do the same   exact thing here paste that in there and   increment that and there you go now we   have some attributes for our animals as   well as the Constructors and the ability   to go in and increase the number of   animals that we have total let's go in   here and create a couple other different   methods for our animals to be able to   operate with we'll go public this guy   right here is going to just print out a   bunch of information about all of the   animal objects and this is how we define   our methods   so we'll say return string another thing   it's very important to understand is a   static method is not going to be able to   access non-static class members so this   guy right here isn't going to be able to   access these attribute values up here   it's going to only be able to get this   because it is static put that in back   your mind   so this guy right here is going to   return a string so I'm going to say   string and I'm going to format this guy   and I'm going to get all the individual   pieces of information about my different   animals this is going to be the name of   my animal and it's so many inches tall   weighs and I'm going to get the amount   that it weighs pounds and likes to say   and then whatever its sound is if I want   to get access to those things I just go   name height weight and sound it's   automatically going to know what animal   I'm specifically talking about so I'm   going to jump down in the main section   and actually create an animal object   pretty easy to do just go animal and I'm   going to tall my animal spot equal to   new animal and let's just say 1510 name   is spot as well as wolf is his sound   that he makes there you go just created   a brand new animal object and it's   called this constructor that we created   up above now you can come in here and   actually get the different items for our   animal print them out here on our screen   and we're just going to do it just like   we would normally do it says and that if   we wanted to get our specific animal   object named spots name we just put spot   dot operator name   and if we want to get his sound they   just go like that if we want to call the   static method that we created previously   let's go and copy this paste that there   you could say number of animals and to   call the static method you're going to   type in the class name followed by get   numb of animals its name and that's how   that works so you don't use spot you   need the class name before that because   that static method belongs to the class   it doesn't belong to the objects and we   could also come in here and call the   objects method by going spot and then to   string is the name of that guy and we   don't put it inside of quotes and that's   going to output all that information   about our animals and you can see right   here spot says wolf number of animals   created as one spot is 15 inches tall   weighs 10 pounds and likes this a wolf   okay so there we go pretty simple   example but an overview of a lot of   things so I showed you how to create an   object method right here with two   strings let's say that we wanted to also   explore something called method   overloading what this is going to allow   us to do is actually work with different   attribute types but also use this same   method name so let me just show you so   let's say we want to let's say we have a   really smart animal that can perform   addition I'm going to create a new   method inside of here a function   whatever you want to call it called get   some and it's going to get an int and   it's going to be called number one if   you want to give a default value to   assign if no value was assigned you   would just type in this like this is   equal to like that all right so that's   going to create a default value of one   if nothing is assigned then what this   guy is going to do is return I'm 1 plus   num2   right like that pretty simple function   but we can also what the overloading   part means is as long as we have   different attributes data types we're   going to also be able to accept doubles   and then also return doubles but you   have to have different attributes inside   of here but we're just going to go   double this this is fine if you just had   double an int that would work but in   this situation I'll just do two doubles   like that and now what I did was I set   it up so that we'll be able to call get   some and pass in two integers or get   some and pass in two doubles it's still   going to work   called method overloading and to see   exactly how that works we could just   come in here like that and then go to   spot dot get some like this and then   pass in a 1 and a 2 and know that that's   going to work but also come in and pass   in one point four two point seven and   also know that that's going to work   another thing that might come of use is   you could also come in here and pass in   these attributes in the in correct order   what we have to do in that situation is   go and say which do we want to assign to   so this accepts num1 and num2 we could   go like this and num1 and then a colon   like that and that's going to work   perfectly well you can also create   objects using what is called the object   initializer so let's say we want to   create another animal called grover just   go new animal like that and then come in   and assign all these values individually   so equals to grover and it's sound is   equal to and it could put goro or   something like that and then all you   have to do is at the very end of this   put a semicolon there's another way to   create objects and see short and another   thing we could do is we can create a   subclass of animal and that subclass is   going to get all of the attributes that   are defined and methods that are defined   in our animal class and we'll call this   one dog so we want to come out of this   guy all together we don't want to be in   this class at all being the same   namespace that's perfectly fine so down   inside of here if we want to create a   class that is going to inherit all the   attributes and methods in the animal   class we just put the colon erin animal   there we go it's pretty simple we could   also come in and of course add in our   own new attributes that only dogs are   going to have and we could say get in   set that's automatically going to   generate all that for us   we could also go and create a custom   constructor that's going to set favorite   food and we go public dog name of the   class right there and if we want to also   be able to call the superclass we would   go :   and base that's going to allow all the   initializations that's in the superclass   constructor all this stuff is still   going to be taking place so that's   really useful because then we'll just   have to come in and go this book   favorite food like that is equal to and   then the default is going to be no   favorite and then this because this guy   right here is there it's going to   initialize all the other different parts   to exactly what was assigned in the   animal constructor so that's pretty   useful this guy right here it's going to   call the default one right there we're   also going to be able to come in and   create a more elaborate constructor   that's going to set values that are   passed inside by going dog and we're   going to find all the different things   are going to be passed aside so we have   our height and our way and our favorite   food and make sure that the string is   defined inside of there there we go so   this constructor is going to accept   everything that's in the first one and   also get favorite food now if we want   all of this to be passed into the   constructor in the animal we just go   base and then pass in height weight name   and sound and it'll take care of   initializing all of those different guys   so that means all I need to do is come   down here and go this favorite food   right like that is equal to the favorite   food that they passed inside of it so   that's useful we're also going to be   able to override methods in animal   inside of our dog class so let's say we   want to override to string well we just   come up here and copy some of this stuff   because it'll be useful that and paste   that in there so here we have new string   or two string and we want to override it   we want to override a method we type in   new and there we are   now we're overriding it we can leave   pretty much everything else like this   and then we could just say something   like and eats and then throw in another   thing for the favorite food for our   animal and then go favorite food and   it's automatically going to update that   and if we would come in and execute all   this stuff let's go and create a dog   inside of here just show the differences   go create dog let's call him spike new   dog create that just a generic old dog   and then if we want to come in and call   the to string on that just to show that   the to string is going to work and we'll   be able to   to call and set off everything   initialize everything using the base   class of animal do that and we could   also overwrite that by going spiked new   dog and then pass in some information   about our dog so 15 spike and ger   whatever and then finally let's say that   he likes chicken or something there we   go   and we created spike in two different   ways and we could go and call to string   again print that out and if we execute   it you're going to see here is where we   created spike with no name and there's   all that information here spike with all   of the information and you can also see   each chickens on there there you go guys   that is how we create or use inheritance   where we inherited every attribute and   method inside the animal class into work   dog class another thing that's   interesting is we're going to be able to   implement what is called polymorphism   through the use of an abstract class we   could also do the same with an interface   but let's go and show you exactly what   an abstract class and interface look   like so let's come down here after dog   is all closed out very much at the end   basically an abstract class is just   going to define methods that must be   defined by those classes that derive   from it one thing to remember is you're   only going to be able to inherit one   abstract class per class however with an   interface you'd be able to inherit   multiple interfaces another thing it's   important to remember is you can't   instantiate or create an object out of   an abstract class so let's come in here   and let's create an abstract class like   this and its name is shape now we're   just going to define kind of like what   this method is going to look like it's   public abstract it's going to return a   double and it is going to be called area   so we're going to be able to create   different area calculations depending   upon the type of shape we're using   another thing it's important to know is   an abstract class can actually contain   methods that do things or non-abstract   class so we could say something like   public void say hi something completely   silly but whatever and then we could say   console.
writeline and then it's just   going to print out hello on the screen   can't do that with interface interfaces   only can contain abstract classes or   methods like that I mean so only   abstract methods inside interfaces but   an   class can actually contain real methods   that do other things and we come down   here and go and create into your face   just so you know what it looks like so   let's create interface public interface   and let's call this shape item just to   have it be something different and every   single method inside of an interface is   an abstract method so you don't need to   put the abstract part inside of there   all you need to do is go double area   like that and there you go you have an   interface and it's going to say that   anyone it wants to inherit this   interface must have a method inside of   it called area seems true up here with   shape however of course it's not going   to require that you have a se Aiye   method only if it has abstract like that   now what all this means is we're going   to be able to come in here and create a   new class and let's call it rectangle   and we want to inherit from the shape   abstract class well that's how hard that   was now you have the requirement to come   in here and use the area it's actually   going to give you an error saying hey   you need to do some stuff and you need   to put the shape area edit edit ah you   click on show potential fixes and click   on implement class and they're dead it   automatically threw it in there now what   I can do inside of my rectangle class   that I have right here is go private   double and a link like that and then   private double width then come into the   area part right here and say it's going   to receive a double number one and   another double number two and then it's   going to assign those values to link but   is equal to num 1 and width is equal to   num - there we go   well actually this isn't the right one   Stu that with let's just come in here   and just get in you clicking around   doing so many different things what   we're gonna do is this is going to be   the constructor sorry about that I'm   sure it didn't confuse you and then   we'll put the double inside of here like   this let's just get rid of that area is   not going to receive anything there we   go now we got that inside of there now   we'll be able to come in here and put   the length and number one inside of   there there you go that's the   constructor you'll call when you create   rectangle objects and then the area   method that we have inside of here what   it's going to do overwrite any time you   want to override a method must put   override inside of there and then we'll   say return length times width and there   we go now we have a rectangle that's   going to calculate area for us but the   reason we want to set this up right here   is we want to also be able to calculate   using other different types of shapes   another different type of shape we're   going to work with here is a triangle   it's coffee a lot of that so I'm going   to change the name from rectangle to   triangle still going to be a shape it's   going to instead receive a base I can't   use the word base because that's a key   word and C sharp so I'm going to just   type in the base and here I'm going to   type in height and this is going to be   triangle this is the constructor again   it's going to receive two numbers just   like before the only difference is in   this situation it's going to say the   base like this and then this guy right   here is going to say the height and then   I'm going to be able to come down inside   of here and leave area remember it has   to use area I'm just going to calculate   area in a little bit different way we're   going to go point five times and then   we'll go to the base like that times   height and you can see that we have two   different shape classes here that are   going to allow us to calculate area in   different ways well I'm here in   rectangle when we talk about operator   overloading   kind of cool basically if we wanted to   be able to figure out ways to use a   operator of addition to add rectangles   together could we do that yes we could   so how you do it's practically pretty   easy we're going to go public this is   going to be a static method it's going   to return a rectangle and if you want to   overload an operator ego operator and   then you follow that with whatever   operator went over lit so there's a plus   that's when I want to overload and what   this is going to be to do is it's going   to receive rectangle let's just call it   wrecked one and another rectangle will   call it wrecked two and it's going to   define exactly how these two guys are   going to add together so we're going to   say double and I can get my rect length   for my new wreck   tangle I'm creating right here by going   rekt one and getting its length plus   wreck two and getting its length there's   a brand new length for my brand new   rectangle that's going to be used   whenever I want to add rectangle objects   together which is really neat and then   this one's going to be the wit like that   and here I just change this to width   instead and change this the width and   then what I can do is just return a   brand new rectangle object using the   combined length and width or whatever   you'd want to do so that's operator   overloading inside of c-sharp with the   addition but you also do this with   pretty much every other operator that   you can think of really neat so now   let's jump back up here into our main   class and play around with these   different guys we can get rid of all   this and now I'm going to show you how   to implement polymorphism through the   use of an abstract class so I'm going to   create a shape call it rectangle is   equal to new rectangle five five let's   give it that value right there or pass   in those two attributes and then I'm   going to create another one also a shape   but this is going to be a triangle and   we'll just call this tri like that and   here see these are both shapes even   though this one's a rectangle and this   one's going to be a triangle like that   and what's neat is this is polymorphism   it's automatically going to call the   right area method depending upon this   even though these are both shapes and   you'd be able to throw these guys into   shape arrays and do anything you want   because remember array can only accept   one data type to get through a whole   bunch of shapes in there but they're   actually rectangles and triangles and   it's automatically going to calculate   the proper area so let's go and look at   it so we go area like this and to   calculate that just go rect and area   call the area method write that now   we're going to do the same thing for our   triangle this and this will just be try   and then we'll just try inside of here   as well and why don't we take a look at   overloading so we'll go   rectangle where we overloaded the plus   sign so we'll create a new rectangle   let's just go and throw a new inside   here like that and pass in five and five   like that and then we'll add it to   another rectangle and throw five and   five in there like that so we're using   this guy that we overloaded so we'll be   able to add two rectangles together to   create a new rectangle and then we'd be   able to come in here and console right   and say something like combined   rectangle area is going to be equal to   and we'll be able to come in and go   combined rect like that it's going to   work   so cool and you can see right here the   area for the rectangle is 25 the area   for the triangle is 12.
5 and the   combined rectangles have an area of 100   so so many different things covered   hopefully they all made sense and of   course leave any questions you have   below I answer every question I get and   I want to talk about generics and   generic class in general basically why   generics are really cool is with the   generic you don't have to specify the   data type of an element in a class or in   a method it's going to automatically   work let me just show you how they work   make a lot more sense whenever you just   see one let's create a class and let's   call it key value and it's a generic so   I do not know what the data type for the   key or the value are going to be T value   like this and then inside of it I'm   going to go public   Tiki give it the name of key it's going   to have getters and setters but I don't   know the datatype for either one of them   public create another one this is going   to be T value this so it's going to   contain each object of type key value is   going to have a key and value all right   so we have that all set up and we don't   know what their data type is if you want   to create a new one you just go key   value call the constructor just like   anything else   TK give it K and then T V once again   finding these bizarre datatypes we're   not giving real data types here but they   will get real here in a second and then   we could say something like ki is equal   to K and then value is equal to V see   we've gone quite some time here and we   still don't know the data type we're   dealing with that's because we can deal   with pretty much any data type we could   then go public and show data for our   method and then here we could throw out   some information on our items we could   say something like 0 is 1 and then we   could grab the key by going this key and   this value that's going to shoot that   out on the screen and that's enough   information you of course know how to go   and create methods and do additional   things here so let's go back up inside   of main right here with this guy and   let's create a new key value class   object and here is where we're going to   define what we're passing inside so   we're going to say hey we're passing in   a string to strings actually one for the   key one for the value and we'll call   this key value object Superman and say   new and then we'll go key value and then   we'll pass in string and string I have   to have the same thing there we could   pass in nothing in regards to values   like that and there you go and then we   could also go in and go Superman key is   equal to and have the key for Superman   be the word Superman and Superman dot   value be equal to Clark Kent and that is   how easy it would be to create that and   we could also create ones with two   different other data types or any type   of data type so it's create another one   and this one we'll call it Samsung TV   like this and instead of having a string   here we could have this be an integer   and then this could be an integer we   could change them both to integers I   don't need them need to be strings we   then have to assign value of zero to   that guy right there and then Samsung TV   and we'd have to assign a key for it but   this is an integer in this situation so   1 2 3 4 5 whatever it doesn't matter   Samsung TV and the value for that and we   could say something like a 50   inch Samsung TV to describe that and   then we could come in and get Superman   and call our show data method like that   and then do the same thing for Samsung   TV dot show data like that and execute   it and I had a little bit of an error   here go down here put a calm inside of   there and we'll execute that and you can   see that it automatically went put   Superman's Clark Kent in there as well   as handled the TV port so that is how   you use generic classes okay so now   let's talk about enumerated types now   basically an enumerated type or an enum   is a unique type that's going to have   symbolic names and then associated   values and we're going to create these   guys outside of our class so we're going   to throw them right here and to create   one we just go public you know them and   say we want to do something with   temperature making a microwave that   freezes and boils and does a whole bunch   other things I don't know first thing   came to mind so here it's going to   freeze it's going to have a setting of   low and it's going to a setting of warm   and it's going to have a setting all   boil okay so there is our numerator type   we just created right there we could   save that we could also come in to this   is going to get the default value of   zero one two and three if we wanted to   give us a different value we come in and   have the D value stored at one instead   if you'd like to do that but I'm going   to keep it this way so we save that enum   called temperature now down inside of   main what I'm going to be able to do is   go and call temperature which is the   data type I just created right there   and let's call this Mike temp like that   and I'll be able to set the temperature   for it by calling temperature and then   followed by low just that easily and I   went ahead and threw in a switch   statement here that's going to check the   value of right here and then you know   print out different output depending   upon whatever the value is set for I'm   gonna execute that and you can see right   here it automatically prints out   temperature is low and I could also come   in here and change this to warm and it's   going to also work that way that that a   temperature is warm so that's how enum   work inside of c-sharp now let's take a   look at Struck's now a struct is a   custom type that's going to hold data   for many different data types and on top   of that it can also have methods inside   of it to offer   on that so we can create a struct again   outside of our class and let's say we   have a struct called customers there we   go and I could just go and put some   customer information inside of here   private let's say we want to have a   balance if the customer maybe owes us   money and then also an ID number for our   customer   okay so there's all those different   attributes all of different data types   doesn't matter we could then provide a   way for us to create a customer so it   would receive a string and a double for   our balance and another integer for our   ID for our customer and then if we want   to assign the values we would just do it   this way pretty simple there we go so   that's a way to create our customers   when inside of this struct we could do   all sorts of different things with this   customer information but this situation   we're just going to display information   about the customer and in this situation   we'll just do just some information   we'll just print it out on the screen   and to do so just good name and to get   the name for this struct just like that   and then we could do exactly the same   thing for the balance as well as the ID   exactly like that so there we go the   name balance and ID so we're able to   create customer store customers print   customers and all those other different   things go down create a customer to   create a new customer we just go   customers or its customers is what I   called this struct up here I believe yes   customers so we'll go customers and   we'll have a customer named Bob and to   create a new one very much like you   would create an object that's how it's   set up and then we could go Bob create   the customer call that method to create   him passing Bob 15 let's say he is is   fifteen dollars and 15 cents   and he has an ID of that and then we   could go Bob dot and then show customer   like this and there we go and it prints   out all the information about that   struct or that customer so that's how   structs work inside a c-sharp   covered a lot of stuff here no suck   about delegates now a delegate is going   to be used to pass methods as arguments   to other methods and a delegate can   represent a method   with similar return types as well as a   similar attribute list and once again   something that's probably better to see   so let's go delegate this is how you   create one double and we'll call this   get some and it's gonna receive a double   and another double there we go and we're   defining this outside of our class and   now what we're going to do is we're   going to use this inside of an anonymous   method and let's come down here and   there's lambda expressions regular   anonymous let's just talking about an   anonymous method basically an anonymous   method has no name and its return type   is defined by the return that's actually   used in the method so we're going to use   this delegate here get some and we'll   give ours the value of sum equal to   delegate and then you're just going to   define inside of this the number one and   the number two double number two and   there we go and this is an anonymous   method it doesn't technically have a   name so we can just go num1 and num2   right like this and then if we would   come out of here make sure you put a   semicolon right there we could then come   out and print to the screen right line   and we'd also be able to pass this in as   a method operator or a method attribute   with but just by passing in some using   get some as the data type that we're   passing into a method then we can just   go five plus 10 is equal to right like   this and then call this anonymous method   by going some five and ten like that and   then of course make sure that we wrap   this inside of our main function like   this file save it execute and you're   going to say 5 plus 10 is equal to 15   drops up there on our screen so that's   how you use an anonymous method now   let's look at lambda expressions which   are kind of similar now basically a   lambda expression is used to act as an   anonymous function or an expression tree   now you're going to be able to assign   the lambda expression to a function   instance like this so we go function go   int this is just going to work with   2-inch here and we could say something   like we give us a get some like that to   sign the value to get some and the way   these guys work is you pass in the @   buttes on the left side of the screen so   we're going to give those the value of x   and y you're going to put this a little   equal sign looks like an arrow on the   right side and then inside of here   you're going to be able to perform all   of your different statements that you   want to happen inside of there you can   see and this is a lambda expression now   what we can do all in this one place   make these calculations we can do write   line and then 5 plus 3 is equal to and   then call get some invoke and then pass   in five and three which is going to go   into that expression right there and   print that out on our screen for us   oh yeah 5 plus 3 is equal to 8 right   there another thing that is a little bit   even more cool with lambda expressions   they're very often used with lists so   let's say we have a list and integer   lists like this call it number list   equal to new list throw some values   inside here 5 10 and 25 one heck there   we go so we create our list now we're   going to be able to use this lambda   expression on every single item in this   list like I said before and we're going   to be able to store it in a new list so   we'll go create another list and this is   going to be called odd gnomes equal to   num list list that we created a second   ago we're going to be able to say where   this comes from something called link I   could do tutorials forever and I'm   probably going to do a longer c-sharp   tutorial here soon because there's just   so much about Seashore so what we're   going to do here is we're going to check   to see if any of these guys are ODS and   if they are we're going to store them in   odd numbers like this they do is you go   like this and you check that this is   equal to one if it is then we want to   convert it to a list and then we want to   store it over in odd numbers right there   so we just in this one line we went and   cycle through all the items in the list   checked if any of our odd numbers if   they were then we stored them in this   guy and then we'll throw up a for each   block like this no we could cycle   through all the different items inside   of that list right like that and print   them out on our screen and there you can   say 5 15 and 25 printed out on the   screen then the final thing to talk   about is file i/o and I'm starting to   lose my voice so what I'm going to do   here is just explain exactly what's   going on basically we're going to be   able to use things called stream readers   and stream writers to   will allow us to create text files and   then either read or write data to them   now there's a whole bunch of different   ways of working with file i/o inside of   c-sharp this is just ways of saving text   data so I'm going to do here it's going   to say there's an error here show   potential fixes and that means I need to   include a library and there we are now   that all fixed so I'm going to do now is   I have a string array called customers   if I want to be able to write to this   I'm going to use the stream writer to   create a brand new text file called   customers dot text right like that or   cost X I'm then going to be able to   cycle through all of the different   customer names inside of that   temporarily store it inside of here and   then if I want to write it to the file I   just go and go stream writer and follow   that with write line and write each   individual word inside of there and then   if I wanted to read that information out   of my text file I need a stream reader   which I called s or I have to define   exactly where I want to read it from and   then once again I'm going to cycle   through all the different lines of text   inside of that text file and output them   on our screen and there you go guys that   is how we do so many different things   inside of c-sharp   I wouldn't did a couple more of them I   throat is really rough and messed up   please leave your questions and comments   below otherwise till next time.";

            string temp = Regex.Replace(content, "[^0-9a-zA-Z]+", " ").ToLower();
            temp = Regex.Replace(temp, "[ ]{2,}", " ").ToLower();
            m_words = temp.Split(new char[] { '\r', '\n', ' ' })
                .Where(x => x != string.Empty)
                .Select(x => x.ToLower())
                .GroupBy(x => x)
                .Select(x => new oWordCount() { count = x.Count(), word = x.Key })
                .OrderBy(x => x.word)
                .ToArray();

            #endregion

            f_word_input_KeyDown(m_word_Input, null);
            f_word_labelText_DoubleClick(new Label() { Text = "hello" }, null);
        }

        #region [ WORD ]

        #region

        int m_word_page_number = 1;

        oWordCount[] m_words = new oWordCount[] { };
        string m_word_current = string.Empty;
        List<string> m_word_selected = new List<string>();
        string m_word_content = string.Empty;

        Label m_word_Message;
        FlowLayoutPanel m_word_Result;
        TextBox m_word_Input;
        Panel m_word_Footer;

        Label m_word_PageCurrent;
        Label m_word_PageTotal;
        Label m_word_TotalItems;

        IconButton word_Translater;
        Label word_current_label;

        #endregion

        void f_word_Init()
        {
            m_word_Message = new Label()
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                //BackColor = Color.Red,
                //Text = "m.Log = string.Format(Update bookmark is {0}- {1} -> {2}, mi.Star, mi.Title, SUCCESSFULLY);",
                TextAlign = ContentAlignment.BottomLeft,
                Height = 17,
                Padding = new Padding(9, 0, 0, 0),
            };

            m_word_Result = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown | FlowDirection.LeftToRight,
            };
            m_word_Result.MouseMove += f_form_move_MouseDown;

            m_word_Footer = new Panel()
            {
                Height = 25,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Padding = new Padding(109, 0, 9, 0),
            };
            m_word_Input = new TextBox()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Width = m_text_search_width,
                Location = new Point(7, 2),
                Height = 19
            };
            m_word_Footer.MouseMove += f_form_move_MouseDown;
            m_tab_Word.Controls.AddRange(new Control[] {
                m_word_Message,
                m_word_Result,
                m_word_Footer,
                //new Label(){ AutoSize = false, Height = 9, Dock = DockStyle.Top },
            });

            //IconButton btn_saveResult = new IconButton(24) { IconType = IconType.ios_cloud_download, Dock = DockStyle.Left };
            //IconButton btn_tags = new IconButton(24) { IconType = IconType.pricetags, Dock = DockStyle.Left, ToolTipText = "Tags" };
            //IconButton btn_bookmark = new IconButton(22) { IconType = IconType.heart, Dock = DockStyle.Left, ToolTipText = "Bookmark" };

            IconButton btn_next = new IconButton(16) { IconType = IconType.ios_arrow_next, Dock = DockStyle.Right };
            IconButton btn_prev = new IconButton(16) { IconType = IconType.ios_arrow_back, Dock = DockStyle.Right };
            IconButton btn_remove = new IconButton(22) { IconType = IconType.trash_a, Dock = DockStyle.Right, ToolTipText = "Clear words selected" };
            IconButton btn_add_playlist = new IconButton(22) { IconType = IconType.android_add, Dock = DockStyle.Right, ToolTipText = "Add to Playlist" };
            IconButton btn_download_caption_cc = new IconButton(22) { IconType = IconType.ios_cloud_download, Dock = DockStyle.Right, ToolTipText = "Download caption|CC & analytic words" };

            m_word_PageCurrent = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Gray,
                Text = "1",
                TextAlign = ContentAlignment.BottomRight,
                Dock = DockStyle.Right,
                Padding = new Padding(9, 3, 0, 0)
            };
            m_word_PageTotal = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Yellow,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            m_word_TotalItems = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Blue,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };

            m_word_Message.MouseMove += f_form_move_MouseDown;
            m_word_Input.KeyDown += f_word_input_KeyDown;
            btn_next.Click += f_word_goPageNextClick;
            btn_prev.Click += f_word_goPagePrevClick;
            btn_add_playlist.MouseClick += f_word_playList_updateClick;
            btn_download_caption_cc.MouseClick += f_word_caption_cc_download_analytic_Click;

            //btn_bookmark.MouseClick += f_word_filter_bookMarkClick;
            //btn_tags.MouseClick += f_word_filter_tagsClick;
            btn_remove.MouseClick += (se, ev) =>
            {
                if (m_word_selected.Count > 0)
                {
                    m_word_Message.Text = string.Format("You clear {0} words selected", m_word_selected.Count);
                    m_word_selected.Clear();
                    foreach (Control li in m_word_Result.Controls)
                        li.BackColor = Color.White;
                }
            };

            word_Translater = new IconButton()
            {
                IconType = IconType.ios_paper_outline,
                Dock = DockStyle.Right,
                ToolTipText = "Translater",
                InActiveColor = Color.DimGray,
            };
            word_Translater.MouseClick += (se, ev) =>
            {
                if (m_media_current_id > 0)
                {
                    IconButton ico = (IconButton)se;
                    if (ico.InActiveColor == Color.DimGray)
                    {
                        ico.InActiveColor = Color.Orange;
                        app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_WORD_TRANSLATER, Input = m_media_current_id });
                    }
                }
            };

            IconButton w_ico_list_selected = new IconButton()
            {
                Dock = DockStyle.Right,
                IconType = IconType.android_done_all,
                ToolTipText = "List words selected",
            };
            w_ico_list_selected.MouseClick += f_word_list_selected_Click;
            IconButton w_ico_repeat = new IconButton(22)
            {
                Dock = DockStyle.Left,
                IconType = IconType.android_sync,
                ToolTipText = "Repeat Play"
            };
            w_ico_repeat.MouseClick += f_word_speak_repeat_Click;

            IconButton w_ico_prev = new IconButton(19)
            {
                Dock = DockStyle.Left,
                IconType = IconType.ios_arrow_back
            };
            w_ico_prev.MouseClick += f_word_speak_prev_Click;
            IconButton w_ico_play = new IconButton()
            {
                Dock = DockStyle.Left,
                IconType = IconType.ios_play_outline
            };
            w_ico_play.MouseClick += f_word_speak_play_Click;
            IconButton w_ico_next = new IconButton(19)
            {
                Dock = DockStyle.Left,
                IconType = IconType.ios_arrow_next
            };
            w_ico_next.MouseClick += f_word_speak_next_Click;
            word_current_label = new Label()
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                //BackColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = font_Title,
            };

            m_word_Footer.Controls.AddRange(new Control[] {
                #region
                 
                word_current_label,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 2 },
                w_ico_repeat,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 2 },
                w_ico_next,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 2 },
                w_ico_play,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 2 },
                w_ico_prev,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 2 },
                m_word_Input,

                word_Translater,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                w_ico_list_selected,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_add_playlist,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_remove,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_download_caption_cc,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_prev,
                m_word_PageCurrent,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "|",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_word_PageTotal,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "_",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_word_TotalItems,
                new Label(){ Dock = DockStyle.Right, Padding = new Padding(0,3,0,0), Text = " words ", TextAlign = ContentAlignment.BottomLeft, AutoSize = true, },
                btn_next,

                #endregion
            });
        }

        private void f_word_caption_cc_download_analytic_Click(object sender, MouseEventArgs e)
        {
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_WORD_CAPTION_CC_DOWNLOAD_ANALYTIC, Input = m_media_current_id });
        }

        private void f_word_speak_prev_Click(object sender, MouseEventArgs e)
        {
        }

        private void f_word_speak_next_Click(object sender, MouseEventArgs e)
        {
        }

        private void f_word_speak_play_Click(object sender, MouseEventArgs e)
        {
        }

        private void f_word_speak_repeat_Click(object sender, MouseEventArgs e)
        {
        }

        private void f_word_list_selected_Click(object sender, MouseEventArgs e)
        {
            IconButton el = (IconButton)sender;
            if (el.InActiveColor == Color.Orange)
            {
                el.InActiveColor = Color.DimGray;
                f_media_loadWord();
            }
            else
            {
                if (m_word_selected.Count > 0)
                {
                    el.InActiveColor = Color.Orange;
                    f_word_draw_Items(m_word_selected.ToArray());
                }
            }
        }

        private void f_word_playList_updateClick(object sender, MouseEventArgs e)
        {
        }

        void f_word_goPage(int page_current)
        {
            if (page_current <= 0) return;
            oWordCollectionResult wr = null;

            string key = m_word_Input.Text.Trim();
            if (key.Length > 0)
            {
                wr = api_word.f_find_Items(key, page_current);
                m_word_Message.Text = string.Format("The finding [{0}] have {1} words", key, wr.Counter);
            }
            else
            {
                wr = api_word.f_get_Items(page_current);
                m_word_Message.Text = string.Empty;
            }

            if (page_current > wr.PageTotal) return;

            m_word_page_number = page_current;
            m_word_TotalItems.Text = wr.Total.ToString();
            m_word_PageTotal.Text = wr.PageTotal.ToString();
            m_word_PageCurrent.Text = page_current.ToString();

            f_word_draw_Items(wr.Words);
        }

        void f_word_draw_Items(string[] words)
        {
            m_word_Result.Controls.Clear();
            if (words.Length == 0) return;

            Control[] names = new Control[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                Label lbl = new Label()
                {
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = words[i],
                    Font = font_Title,
                    Height = 17,
                    Padding = new Padding(0),
                    Margin = new Padding(19, 3, 0, 0),
                    BackColor = m_word_selected.IndexOf(words[i]) == -1 ? Color.White : Color.Orange,
                };
                names[i] = lbl;
                lbl.MouseClick += f_word_labelText_MouseClick;
                lbl.DoubleClick += f_word_labelText_DoubleClick;
            }
            m_word_Result.Controls.AddRange(names);
        }

        private void f_word_labelText_DoubleClick(object sender, EventArgs e)
        {
            Control uc = (Control)sender;
            string text = uc.Text;
            m_word_current = text;

            m_tab.SelectedItem = m_tab_WordDetail;

            uc.BackColor = Color.Orange;
            if (m_word_selected.IndexOf(text) == -1)
                m_word_selected.Add(text);
        }

        private void f_word_labelText_MouseClick(object sender, MouseEventArgs e)
        {
            Control lbl = (Control)sender;
            string text = lbl.Text;

            if (lbl.BackColor == Color.White)
            {
                m_word_current = text;
                word_current_label.Text = text;

                lbl.BackColor = Color.Orange;
                if (m_word_selected.IndexOf(text) == -1)
                    m_word_selected.Add(text);
            }
            else
            {
                lbl.BackColor = Color.White;
                if (m_word_selected.IndexOf(text) != -1)
                    m_word_selected.Remove(text);

                m_word_current = string.Empty;
                word_current_label.Text = string.Empty;

                if (m_word_selected.Count > 0)
                {
                    m_word_current = m_word_selected[m_word_selected.Count - 1];
                    word_current_label.Text = m_word_selected[m_word_selected.Count - 1];
                }
            }
        }

        private void f_word_goPagePrevClick(object sender, EventArgs e)
        {
            f_word_goPage(m_word_page_number - 1);
        }

        private void f_word_goPageNextClick(object sender, EventArgs e)
        {
            f_word_goPage(m_word_page_number + 1);
        }

        private void f_word_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null || e.KeyCode == Keys.Enter)
                f_word_goPage(1);
        }

        #endregion

        #region [ WORD DETAIL ]

        Panel wd_header = null;
        Panel wd_content = null;

        Label wd_word_name = null;
        Label wd_word_pronunciation = null;
        Label wd_word_meaning = null;
        RichTextBoxEx wd_text_detail = null;

        IconButton wd_word_speak = null;

        AxWindowsMediaPlayer wd_media;
        Panel wd_footer;

        void f_word_detail_Init()
        {
            wd_header = new Panel()
            {
                Height = 45,
                Dock = DockStyle.Top,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(9, 7, 0, 0),
            };
            wd_content = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                //BackColor = Color.Orange,
                Padding = new Padding(9, 0, 0, 0),
            };

            wd_word_name = new Label()
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "hello",
                Font = new Font("Arial", 17f, FontStyle.Bold),
            };
            wd_word_pronunciation = new Label()
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "/heˈləʊ/",
                Font = new Font("Arial", 13f, FontStyle.Regular),
                Padding = new Padding(9, 3, 9, 0),
            };
            wd_word_meaning = new Label()
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Xin chào",
                ForeColor = Color.DimGray,
                Font = new Font("Arial", 9f, FontStyle.Italic),
                Padding = new Padding(0, 7, 0, 0),
            };

            wd_word_speak = new IconButton(32)
            {
                IconType = IconType.ios_volume_high,
                Dock = DockStyle.Right
            };
            wd_word_speak.MouseClick += f_wd_word_detail_speakClick;

            wd_text_detail = new RichTextBoxEx()
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                //BackColor = Color.Yellow,
                Font = font_TextView,
            };
            wd_content.Controls.Add(wd_text_detail);
            wd_text_detail.MouseClick += f_wd_text_detail_Click;


            wd_footer = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 45,
                BackColor = Color.DimGray,
            };

            wd_media = new AxWindowsMediaPlayer()
            {
                //Dock = DockStyle.Right,
                //Width = m_media_width,
                Location = new Point(-999, -999)
            };

            wd_footer.Controls.AddRange(new Control[] {
                wd_media,
            });

            m_tab_WordDetail.Controls.AddRange(new Control[] {
                wd_content,
                wd_header,
                wd_footer,
            });

            wd_header.Controls.AddRange(new Control[] {
                wd_word_meaning,
                wd_word_pronunciation,
                wd_word_name,

                wd_word_speak,
            });

        }

        private void f_wd_text_detail_Click(object sender, MouseEventArgs e)
        {
            //int firstcharindex = wd_text_detail.GetFirstCharIndexOfCurrentLine();
            //int currentline = wd_text_detail.GetLineFromCharIndex(firstcharindex);
            //if (wd_text_detail.Lines.Length >= currentline)
            //{
            //    string currentlinetext = wd_text_detail.Lines[currentline];
            //    wd_text_detail.Select(firstcharindex, currentlinetext.Length);
            //    wd_text_detail.SelectionBackColor = Color.Orange;
            //}

            int index = wd_text_detail.GetCharIndexFromPosition(e.Location);
            int line = wd_text_detail.GetLineFromCharIndex(index);
            int lineStart = wd_text_detail.GetFirstCharIndexFromLine(line);
            int lineEnd = wd_text_detail.GetFirstCharIndexFromLine(line + 1);
            if (lineEnd == -1)
            {
                lineEnd = wd_text_detail.TextLength;
            }
            wd_text_detail.SelectionStart = lineStart;
            wd_text_detail.SelectionLength = lineEnd - lineStart;


        }

        private void f_wd_word_detail_speakClick(object sender, MouseEventArgs e)
        {
            //string url = api_media.f_word_speak_getURL(m_word_current);
            //if (!string.IsNullOrEmpty(url))
            //    wd_media.URL = url;
            if (!string.IsNullOrEmpty(m_word_current))
            {
                string[] urls = api_media.f_word_speak_getURLs(m_word_current);
                var playlist = wd_media.playlistCollection.newPlaylist("playlist_" + m_word_current);

                for (int Count = 0; Count < urls.Length; Count++)
                {
                    var media = wd_media.newMedia(urls[Count]);
                    playlist.appendItem(media);
                }

                wd_media.currentPlaylist = playlist;

                wd_media.Ctlcontrols.play();
            }
        }

        void f_word_detail_Active()
        {
            if (!string.IsNullOrEmpty(m_word_current)
                && wd_word_name.Text != m_word_current)
            {
                //wd_word_name.Text = string.Empty;

                wd_word_name.Text = m_word_current;
                wd_word_pronunciation.Text = string.Empty;
                wd_word_meaning.Text = string.Empty;



                //wd_word_meaning.Text = api_media.f_word_meaning_Vi(m_word_current);

                //if (string.IsNullOrEmpty(m_word_content))
                //    m_word_content = api_media.f_media_getText(m_media_current_id);

                //string[] sentences = api_media.f_media_getSentencesByWord(m_media_current_id, m_word_current);
                //if (sentences.Length > 0)
                //    wd_text_detail.Text = string.Join(Environment.NewLine + Environment.NewLine, sentences);

                //new Thread(new ThreadStart(() =>
                //{
                //    string s = api_media.f_word_speak_getPronunciation(m_word_current, false);
                //    if (!string.IsNullOrEmpty(s))
                //    {
                //        if (s.Contains('{') && s.Contains('}'))
                //            s = string.Join(string.Empty, s.Split(new char[] { '{', '}' }).Where((x, k) => k != 1).ToArray());

                //        //if (s.Contains('/'))
                //        //{
                //        //    wd_word_pronunciation.crossThreadPerformSafely(() =>
                //        //    {
                //        //        wd_word_pronunciation.Text = s.Split('/')[1];
                //        //    });
                //        //}

                //        wd_text_detail.crossThreadPerformSafely(() =>
                //        {
                //            wd_text_detail.Text = s;
                //            wd_text_detail.SelectAll();
                //            wd_text_detail.SelectionParaSpacing = new YYProject.RichEdit.RTBParaSpacing(0, 150);
                //            wd_text_detail.Select(0, 0);
                //        });
                //    }
                //})).Start();
            }
        }

        #endregion

        #region [ AUDIO ]

        IconButton btn_play;
        private AxWindowsMediaPlayer m_media;
        private const int m_media_width = 215;
        private Label lbl_hide_border_left;

        void f_audio_initUI()
        {
            //m_msg_api = new Label()
            //{
            //    Dock = DockStyle.Bottom,
            //    Text = "English Media",
            //    TextAlign = ContentAlignment.TopLeft,
            //    AutoSize = false,
            //    Height = 15,
            //    Padding = new Padding(5, 0, 0, 0),
            //};

            lbl_hide_border_left = new Label()
            {
                AutoSize = false,
                Width = 3,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            this.Controls.Add(lbl_hide_border_left);
            lbl_hide_border_left.MouseMove += f_form_move_MouseDown;

            // MEDIA
            m_media = new AxWindowsMediaPlayer();
            m_media.Enabled = true;
            m_media.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(this.f_media_event_PlayStateChange);
            m_media.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Controls.Add(m_media);

            btn_play = new IconButton(24)
            {
                IconType = IconType.ios_play_outline,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Visible = false,
                Width = m_media_width * 2 - 100,
                Height = 43,
            };
            btn_play.MouseMove += f_form_move_MouseDown;
            btn_play.Click += f_audio_play_MouseClick;
            this.Controls.Add(btn_play);
        }

        private void f_audio_play_MouseClick(object sender, EventArgs e)
        {
            if (m_media_current_id > 0)
            {
                btn_play.Visible = false;
                if (string.IsNullOrEmpty(m_media.URL))
                {
                    f_video_openMp3_Request();
                }
                else
                {
                    m_media.Visible = true;
                    m_media.Ctlcontrols.play();
                }
            }
        }

        #endregion

        #region [ TAB ]

        #region

        private FATabStrip m_tab;
        private FATabStripItem m_tab_Store;
        private FATabStripItem m_tab_Search;
        private FATabStripItem m_tab_WordDetail;
        private FATabStripItem m_tab_Listen;
        private FATabStripItem m_tab_Pronunce;
        private FATabStripItem m_tab_Word;
        private FATabStripItem m_tab_Grammar;
        private FATabStripItem m_tab_Text;
        private FATabStripItem m_tab_Writer;
        private FATabStripItem m_tab_Book;
        private FATabStripItem m_tab_Browser;

        //☆★☐☑⧉✉⦿⦾⚠⚿⛑✕✓⥀✖↭☊⦧▷◻◼⟲≔☰⚒❯►❚❚❮⟳⚑⚐✎✛
        //🕮🖎✍⦦☊🕭🔔🗣🗢🖳🎚🏷🖈🎗🏱🏲🗀🗁🕷🖒🖓👍👎♥♡♫♪♬♫🎙🎖🗝●◯⬤⚲☰⚒🕩🕪❯►❮⟳⚐🗑✎✛🗋🖫⛉ ⛊ ⛨⚏★☆
        const string tab_caption_store = "☰";
        const string tab_caption_search = "⚲";
        const string tab_caption_word = "W";
        const string tab_caption_word_detail = "WD";
        //const string tab_caption_word_detail = "⛉";
        const string tab_caption_listen = "►";
        const string tab_caption_pronunce = "P"; //☊
        const string tab_caption_writer = "✍";
        const string tab_caption_grammar = "Grammar";
        const string tab_caption_text = "Text";
        const string tab_caption_book = "Book";
        const string tab_caption_browser = "Net";

        #endregion

        void f_tab_initUI()
        {
            lbl_title = new Label()
            {
                AutoSize = false,
                Text = "",
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(55, 0),
                //BackColor = Color.DimGray,
            };
            lbl_title.MouseMove += f_form_move_MouseDown;
            this.Controls.Add(lbl_title);

            btn_exit = new IconButton(20) { IconType = IconType.ios_close_empty, Anchor = AnchorStyles.Left | AnchorStyles.Top };
            btn_exit.Click += (se, ev) => { app.Exit(); };
            this.Controls.Add(btn_exit);

            btn_mini = new IconButton(20) { IconType = IconType.ios_minus_empty, Anchor = AnchorStyles.Left | AnchorStyles.Top };
            btn_mini.Click += (se, ev) => { this.WindowState = FormWindowState.Minimized; };
            this.Controls.Add(btn_mini);

            //////////////////////////////////////////////////////////
            // TAB


            m_tab = new FATabStrip()
            {
                Dock = DockStyle.Fill,
                AlwaysShowClose = false,
                AlwaysShowMenuGlyph = false,
                Margin = new Padding(0, 45, 0, 0),
            };

            m_tab_Store = new FATabStripItem(tab_caption_store, false);
            m_tab_Search = new FATabStripItem(tab_caption_search, false);
            m_tab_WordDetail = new FATabStripItem(tab_caption_word_detail, false);
            m_tab_Pronunce = new FATabStripItem(tab_caption_pronunce, false);
            m_tab_Listen = new FATabStripItem(tab_caption_listen, false);
            m_tab_Writer = new FATabStripItem(tab_caption_writer, false);
            m_tab_Grammar = new FATabStripItem(tab_caption_grammar, false);
            m_tab_Word = new FATabStripItem(tab_caption_word, false);
            m_tab_Text = new FATabStripItem(tab_caption_text, false);
            m_tab_Text.Visible = false;
            m_tab_Book = new FATabStripItem(tab_caption_book, false);
            m_tab_Browser = new FATabStripItem(tab_caption_browser, false);
            m_tab.TabStripItemSelectionChanged += f_tab_selectChanged;
            m_tab.Items.AddRange(new FATabStripItem[] {
                m_tab_Store,
                m_tab_Search,
                m_tab_Listen,
                m_tab_Pronunce,
                m_tab_Writer,
                m_tab_Book,
                m_tab_Grammar,
                m_tab_Word,
                m_tab_WordDetail,
                m_tab_Text,
                m_tab_Browser,
            });
            Label lbl_bgHeader = new Label() { Dock = DockStyle.Top, Height = lbl_title.Height };
            m_tab.MouseMove += f_form_move_MouseDown;
            lbl_bgHeader.MouseMove += f_form_move_MouseDown;
            this.Controls.AddRange(new Control[] {
                m_tab,lbl_bgHeader
                //,m_msg_api
            });

            m_media_text = new TextBox()
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.Vertical,
                Font = font_Title
            };
            m_tab_Text.Padding = new Padding(9, 0, 0, 0);
            m_tab_Text.Controls.Add(m_media_text);
        }

        private void f_tab_selectChanged(TabStripItemChangedEventArgs e)
        {
            if (e.Item == null) return;
            switch (e.Item.Caption)
            {
                case tab_caption_store: // "☰"
                    break;
                case tab_caption_search: // "⚲"
                    break;
                case tab_caption_listen: // "►"
                    break;
                case tab_caption_pronunce: // "☊"
                    break;
                case tab_caption_writer: // "✍"
                    break;
                case tab_caption_grammar: // "Grammar"
                    break;
                case tab_caption_word: // "Word"
                    #region

                    if (m_word_Result != null && m_word_Result.Controls.Count == 0)
                        f_word_goPage(1);

                    ////if (m_tab_Text.Tag != null && (long)m_tab_Word.Tag != m_media_current_id)
                    //if ((m_tab_Word.Tag == null && m_media_current_id > 0)
                    //    || (m_tab_Word.Tag != null
                    //    && (long)m_tab_Word.Tag != m_media_current_id
                    //    && m_media_current_id > 0))
                    //{
                    //    f_media_loadWord();
                    //    m_tab_Word.Tag = m_media_current_id;
                    //}

                    break;
                case tab_caption_word_detail: // "⛉"
                    f_word_detail_Active();
                    break;

                #endregion
                case tab_caption_text: // "Text"
                    #region

                    if ((m_tab_Text.Tag == null && m_media_current_id > 0)
                        || (m_tab_Text.Tag != null
                        && (long)m_tab_Text.Tag != m_media_current_id
                        && m_media_current_id > 0))
                    {
                        f_media_loadText();
                        m_tab_Text.Tag = m_media_current_id;
                    }
                    break;

                #endregion
                case tab_caption_book: // "Book"
                    break;
                case tab_caption_browser: // "Net"
                    break;
            }
        }

        #endregion

        #region [ STORE ] 

        #region

        bool m_store_caption = false;
        msg m_store_current_msg = null;
        long m_store_item_current_id = 0;
        string m_store_item_current_text = string.Empty;

        Label m_store_Message;
        Panel m_store_Result;
        TextBox m_store_Input;
        Panel m_store_Footer;

        Label m_store_PageCurrent;
        Label m_store_PageTotal;
        Label m_store_TotalItems;

        #endregion

        void f_store_initUI()
        {
            m_store_Message = new Label()
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                //BackColor = Color.Red,
                //Text = "m.Log = string.Format(Update bookmark is {0}- {1} -> {2}, mi.Star, mi.Title, SUCCESSFULLY);",
                TextAlign = ContentAlignment.BottomLeft,
                Height = 17,
                Padding = new Padding(9, 0, 0, 0),
            };

            m_store_Result = new Panel()
            {
                AutoScroll = true,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
            };
            m_store_Result.MouseMove += f_form_move_MouseDown;

            m_store_Footer = new Panel()
            {
                Height = 25,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Padding = new Padding(109, 0, 9, 0),
            };
            m_store_Input = new TextBox()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Width = m_text_search_width,
                Location = new Point(7, 2),
                Height = 19
            };
            m_store_Input.KeyDown += f_store_input_KeyDown;
            m_store_Footer.MouseMove += f_form_move_MouseDown;
            m_tab_Store.Controls.AddRange(new Control[] {
                m_store_Message,
                m_store_Result,
                m_store_Footer,
                //new Label(){ AutoSize = false, Height = 9, Dock = DockStyle.Top },
            });

            IconButton btn_saveResult = new IconButton(24) { IconType = IconType.ios_cloud_download, Dock = DockStyle.Left };
            IconButton btn_tags = new IconButton(24) { IconType = IconType.pricetags, Dock = DockStyle.Left, ToolTipText = "Tags" };
            IconButton btn_user = new IconButton(22) { IconType = IconType.person, Dock = DockStyle.Left, ToolTipText = "User" };
            IconButton btn_channel = new IconButton(22) { IconType = IconType.android_desktop, Dock = DockStyle.Left, ToolTipText = "Channel" };
            IconButton btn_bookmark = new IconButton(22) { IconType = IconType.heart, Dock = DockStyle.Left, ToolTipText = "Bookmark" };
            IconButton btn_caption = new IconButton(22) { IconType = IconType.closed_captioning, Dock = DockStyle.Left, ToolTipText = "Caption|CC" };

            IconButton btn_next = new IconButton(16) { IconType = IconType.ios_arrow_next, Dock = DockStyle.Right };
            IconButton btn_prev = new IconButton(16) { IconType = IconType.ios_arrow_back, Dock = DockStyle.Right };
            IconButton btn_remove = new IconButton(22) { IconType = IconType.trash_a, Dock = DockStyle.Right };
            IconButton btn_add_playlist = new IconButton(22) { IconType = IconType.android_add, Dock = DockStyle.Right, ToolTipText = "Add to Playlist" };

            m_store_PageCurrent = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Gray,
                Text = "1",
                TextAlign = ContentAlignment.BottomRight,
                Dock = DockStyle.Right,
                Padding = new Padding(9, 3, 0, 0)
            };
            m_store_PageTotal = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Yellow,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            m_store_TotalItems = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Blue,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            btn_next.Click += f_store_goPageNextClick;
            btn_prev.Click += f_store_goPagePrevClick;

            btn_caption.MouseClick += f_store_caption_Click;
            btn_bookmark.MouseClick += f_store_filter_bookMarkClick;
            btn_tags.MouseClick += f_store_filter_tagsClick;
            btn_channel.MouseClick += f_store_filter_channelClick;
            btn_user.MouseClick += f_store_filter_userClick;

            btn_add_playlist.MouseClick += f_store_playList_updateClick;
            btn_remove.MouseClick += f_store_media_removeClick;

            m_store_Message.MouseMove += f_form_move_MouseDown;
            m_store_Footer.Controls.AddRange(new Control[] {
                #region

                //m_store_Message,
                btn_caption,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_bookmark,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_channel,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_user,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_tags,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                //btn_saveResult,
                //new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                m_store_Input,

                btn_add_playlist,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_remove,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_prev,
                m_store_PageCurrent,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "|",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_store_PageTotal,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "_",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_store_TotalItems,
                new Label(){ Dock = DockStyle.Right, Padding = new Padding(0,3,0,0), Text = " items ", TextAlign = ContentAlignment.BottomLeft, AutoSize = true, },
                btn_next,

                #endregion
            });
        }

        private void f_store_caption_Click(object sender, MouseEventArgs e)
        {
            IconButton cap = (IconButton)sender;
            if (m_store_caption)
            {
                m_store_caption = false;
                cap.InActiveColor = Color.DimGray;
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_STORE, Input = m_store_Input.Text.Trim(), Log = m_store_caption ? "CC" : string.Empty });
            }
            else
            {
                m_store_caption = true;
                cap.InActiveColor = Color.Orange;
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_STORE, Input = m_store_Input.Text.Trim(), Log = m_store_caption ? "CC" : string.Empty });
            }
        }

        private void f_store_media_removeClick(object sender, MouseEventArgs e)
        {
        }

        private void f_store_playList_updateClick(object sender, MouseEventArgs e)
        {
        }

        private void f_store_filter_userClick(object sender, MouseEventArgs e)
        {
        }

        private void f_store_filter_channelClick(object sender, MouseEventArgs e)
        {
        }

        private void f_store_filter_tagsClick(object sender, MouseEventArgs e)
        {
        }

        private void f_store_filter_bookMarkClick(object sender, MouseEventArgs e)
        {
            IconButton it = (IconButton)sender;
            if (it.InActiveColor == Color.DimGray)
            {
                it.InActiveColor = Color.Orange;
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_FILTER_BOOKMAR_STAR });
            }
            else
            {
                it.InActiveColor = Color.DimGray;
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_STORE, Input = string.Empty });
            }
        }

        void f_store_draw_Media(List<long> ls)
        {
            m_store_Result.crossThreadPerformSafely(() =>
            {
                m_store_Result.Controls.Clear();
            });

            if (ls.Count == 0) return;

            const int margin_top = 5;
            const int margin_bottom = 5;
            const int margin_left = 9;

            int y = 0, x = 0, row = 0;
            Control[] pics = new Control[30];
            Control[] tits = new Control[30];
            Control[] stars = new Control[30];

            #region

            for (int i = 0; i < ls.Count; i++)
            {
                if (i > 29) break;
                if (i == 0 || i == 1)
                {
                    x = i == 0 ? margin_left : (app.m_box_width + margin_left * 2);
                    y = 0;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        row = i / 2;
                        x = margin_left;
                        y = (app.m_item_height * row) + margin_bottom * row;
                    }
                    else
                    {
                        row = (int)(i / 2);
                        x = app.m_box_width + margin_left * 2;
                        y = (app.m_item_height * row) + margin_bottom * row;
                    }
                }

                oMedia media = api_media.f_media_getInfo(ls[i]);
                if (media == null) continue;

                PictureBox pic = new PictureBox()
                {
                    //Text = i.ToString(),
                    //TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.LightGray,
                    Width = app.m_item_width,
                    Height = app.m_item_height,
                    Location = new Point(x, y + margin_top),
                    Tag = media.Id
                };

                //Bitmap img = api_media.f_image_getCache(media.Id);
                //if (img != null)
                //    pic.Image = img;

                Color bgColor = media.Id == m_media_current_id ? Color.Orange : Color.LightGray;

                Label lbl = new Label()
                {
                    Name = media.Id.ToString(),
                    Text = (i + 1).ToString() + ", " + media.Title,
                    TextAlign = ContentAlignment.MiddleLeft,

                    AutoSize = false,
                    BackColor = bgColor,
                    //ForeColor = Color.Black,
                    Width = app.m_box_width - app.m_item_width,
                    Height = app.m_box_height - app.m_item_height,
                    Location = new Point(pic.Location.X + app.m_item_width, pic.Location.Y),
                    Padding = new Padding(9, 0, 0, 0),
                    Font = font_Title,
                };

                pic.MouseClick += f_store_picVideo_MouseDoubleClick;
                lbl.MouseClick += f_store_labelTitle_MouseClick;
                lbl.MouseMove += f_form_move_MouseDown;
                pic.MouseMove += f_form_move_MouseDown;

                pics[i] = pic;
                tits[i] = lbl;


                IconButton star = new IconButton(19)
                {
                    Name = "star-" + media.Id.ToString(),
                    IconType = IconType.ios_heart_outline,
                    Location = new Point(pic.Location.X + (app.m_box_width - 19), pic.Location.Y + 3),
                    BackColor = bgColor,
                    ActiveColor = Color.Black,
                    Tag = media.Id.ToString() + "|" + media.Title,
                    InActiveColor = media.Star ? Color.Red : Color.DimGray,
                };
                star.MouseClick += f_store_bookmark_MouseClick;
                stars[i] = star;
            }

            #endregion

            m_store_Result.crossThreadPerformSafely(() =>
            {
                m_store_Result.Controls.AddRange(stars);
                m_store_Result.Controls.AddRange(tits);
                m_store_Result.Controls.AddRange(pics);
            });
        }

        void f_store_Result(oMediaSearchLocalResult rs)
        {
            f_store_draw_Media(rs.MediaIds);

            int page = rs.CountResult / rs.PageSize;
            if (rs.CountResult % rs.PageSize != 0) page++;

            m_store_PageCurrent.crossThreadPerformSafely(() =>
            {
                m_store_PageCurrent.Text = rs.PageNumber.ToString();
            });
            m_store_PageTotal.crossThreadPerformSafely(() =>
            {
                m_store_PageTotal.Text = page.ToString();
            });
            m_store_TotalItems.crossThreadPerformSafely(() =>
            {
                m_store_TotalItems.Text = rs.CountResult.ToString();
            });
        }

        private void f_store_goPagePrevClick(object sender, EventArgs e)
        {
            if (m_store_current_msg != null)
            {
                if (m_store_current_msg.PageNumber > 1)
                {
                    m_store_current_msg.PageNumber = m_store_current_msg.PageNumber - 1;
                    app.postToAPI(m_store_current_msg);
                }
            }
        }

        private void f_store_goPageNextClick(object sender, EventArgs e)
        {
            if (m_store_current_msg != null)
            {
                if ((m_store_current_msg.PageNumber - 1) * m_store_current_msg.PageSize < m_store_current_msg.Counter)
                {
                    m_store_current_msg.PageNumber = m_store_current_msg.PageNumber + 1;
                    app.postToAPI(m_store_current_msg);
                }
            }
        }

        private void f_store_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string key = m_store_Input.Text.Trim();
                if (key.Length > 1)
                {
                    m_store_Message.Text = "Finding [" + key + "] ...";
                    app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_STORE, Input = key, Log = m_store_caption ? "CC" : string.Empty });
                }
                else
                    app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_STORE, Input = string.Empty, Log = m_store_caption ? "CC" : string.Empty });

                //m_store_Message.Text = "Length of keywords must be greater than 1 characters.";
            }
        }

        private void f_store_picVideo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ((Control)sender).BackColor = Color.Gray;
            string mid = ((Control)sender).Tag.ToString();

            Control _lbl = m_store_Result.Controls.Find(mid, false).SingleOrDefault();
            if (_lbl != null)
                f_store_labelTitle_MouseClick(_lbl, null);
        }

        private void f_store_labelTitle_MouseClick(object sender, MouseEventArgs e)
        {
            Control it = ((Control)sender);
            long mid = long.Parse(it.Name);
            if (mid == m_media_current_id && e != null) return;

            it.BackColor = Color.Orange;
            Control star_sel = m_store_Result.Controls.Find("star-" + it.Name, false).SingleOrDefault();
            if (star_sel != null)
                star_sel.BackColor = Color.Orange;

            if (m_media_current_id > 0)
            {
                Control itprev = m_store_Result.Controls.Find(m_media_current_id.ToString(), false).SingleOrDefault();
                if (itprev != null)
                    itprev.BackColor = Color.LightGray;
                Control star_prev = m_store_Result.Controls.Find("star-" + m_media_current_id.ToString(), false).SingleOrDefault();
                if (star_prev != null)
                    star_prev.BackColor = Color.LightGray;
            }

            m_store_item_current_id = mid;
            m_store_item_current_text = it.Text;

            m_media_current_id = mid;
            m_media_current_title = it.Text;
            m_media_current_tab = MEDIA_TAB.TAB_STORE;

            m_store_Message.Text = it.Text;
            this.Text = m_media_current_title;
            //lbl_title.Text = m_media_current_title;

            string[] sentences = api_media.f_media_getSentences(m_media_current_id);


            if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying) m_media.Ctlcontrols.stop();

            if (e != null)
            {
                // Only click on label title

                m_media.URL = string.Empty;
                if (m_media.Visible) m_media.Visible = false;
                btn_play.InActiveColor = Color.DimGray;
                btn_play.Visible = true;
                f_video_openMp3_Request();
                app.postToAPI(new msg() { API = _API.WORD, KEY = _API.WORD_KEY_ANALYTIC, Input = m_media_current_id });
            }
            else
            {
                // From picture click -> call click to lable title

                f_video_openMp4_Request();

                m_media.URL = string.Empty;
                if (m_media.Visible) m_media.Visible = false;
                btn_play.InActiveColor = Color.DimGray;
                btn_play.Visible = true;

                f_video_openMp3_Request();
                app.postToAPI(new msg() { API = _API.WORD, KEY = _API.WORD_KEY_ANALYTIC, Input = m_media_current_id, Log = m_media_current_title });
            }
        }

        private void f_store_bookmark_MouseClick(object sender, MouseEventArgs e)
        {
            string tag = ((Control)sender).Tag.ToString(),
                mid = tag.Split('|')[0], title = mid.Length < tag.Length ? tag.Substring(mid.Length + 1) : string.Empty;
            //m_msg_api.Text = "You saved item to bookmark: " + title;
            if (((IconButton)sender).InActiveColor == Color.Red)
                ((IconButton)sender).InActiveColor = Color.DimGray;
            else
                ((IconButton)sender).InActiveColor = Color.Red;
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_UPDATE_BOOKMARK_STAR, Input = long.Parse(mid) });
        }

        #endregion

        #region [ SEARCH ]

        long m_search_item_current_id = 0;
        string m_search_item_current_text = string.Empty;
        bool m_search_caption = false;

        private msg m_search_current_msg = null;
        private TextBox m_search_Input;
        private Panel m_search_Result;
        private Label m_search_PageCurrent;
        private Label m_search_PageTotal;
        private Label m_search_TotalItems;
        private Label m_search_Message;
        private Panel m_search_Footer;

        void f_search_initUI()
        {

            m_search_Message = new Label()
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                TextAlign = ContentAlignment.BottomLeft,
                Height = 15,
                Padding = new Padding(9, 0, 0, 0),
            };

            m_search_Result = new Panel()
            {
                AutoScroll = true,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
            };
            m_search_Result.MouseMove += f_form_move_MouseDown;

            m_search_Footer = new Panel()
            {
                Height = 25,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Padding = new Padding(109, 0, 9, 0),
            };
            m_search_Input = new TextBox()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Width = m_text_search_width,
                Location = new Point(7, 2),
                Height = 19
            };



            m_search_Input.KeyDown += f_search_input_KeyDown;

            m_search_Footer.MouseMove += f_form_move_MouseDown;
            m_tab_Search.Controls.AddRange(new Control[] {
                m_search_Message,
                m_search_Result,
                m_search_Footer,
                //new Label(){ AutoSize = false, Height = 9, Dock = DockStyle.Top }
            });

            IconButton btn_save = new IconButton(24) { IconType = IconType.ios_cloud_download, Dock = DockStyle.Left };
            IconButton btn_stop_search = new IconButton(16) { IconType = IconType.stop, Dock = DockStyle.Left, ToolTipText = "Stop Search" };
            IconButton btn_user = new IconButton(22) { IconType = IconType.person, Dock = DockStyle.Left, ToolTipText = "User" };
            IconButton btn_channel = new IconButton(22) { IconType = IconType.android_desktop, Dock = DockStyle.Left, ToolTipText = "Channel" };
            IconButton btn_caption_filter = new IconButton(22) { IconType = IconType.closed_captioning, Dock = DockStyle.Left, ToolTipText = "Filter by caption|CC" };

            IconButton btn_next = new IconButton(16) { IconType = IconType.ios_arrow_back, Dock = DockStyle.Right };
            IconButton btn_prev = new IconButton(16) { IconType = IconType.ios_arrow_next, Dock = DockStyle.Right };
            IconButton btn_remove = new IconButton(22) { IconType = IconType.trash_a, Dock = DockStyle.Right };
            IconButton btn_add_playlist = new IconButton(22) { IconType = IconType.android_add, Dock = DockStyle.Right, ToolTipText = "Add to Playlist" };

            m_search_PageCurrent = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Gray,
                Text = "1",
                TextAlign = ContentAlignment.BottomRight,
                Dock = DockStyle.Right,
                Padding = new Padding(9, 3, 0, 0)
            };
            m_search_PageTotal = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Yellow,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            m_search_TotalItems = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Blue,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            btn_next.Click += f_search_goPageNextClick;
            btn_prev.Click += f_search_goPagePrevClick;

            btn_save.MouseClick += f_search_saveItemSelected;
            btn_remove.MouseClick += f_search_removeCacheAll;
            btn_stop_search.MouseClick += (se, ev) =>
            {
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_ONLINE_STOP });
            };
            btn_caption_filter.MouseClick += (se, ev) =>
            {
                if (m_search_caption)
                {
                    m_search_caption = false;
                    btn_caption_filter.InActiveColor = Color.DimGray;
                }
                else
                {
                    m_search_caption = true;
                    btn_caption_filter.InActiveColor = Color.Orange;
                }
            };
            m_search_Message.MouseMove += f_form_move_MouseDown;
            m_search_Footer.Controls.AddRange(new Control[] {
                #region

                m_search_Message,
                //btn_channel,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_caption_filter,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_stop_search,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_save,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                m_search_Input,

                //btn_add_playlist,
                //new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                //btn_folder,
                //new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                //btn_caption_filter,
                //new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_remove,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_next,
                m_search_PageCurrent,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "|",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_search_PageTotal,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "_",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_search_TotalItems,
                new Label(){ Dock = DockStyle.Right, Padding = new Padding(0,3,0,0), Text = " items ", TextAlign = ContentAlignment.BottomLeft, AutoSize = true, },
                btn_prev,

                #endregion
            });
        }

        private void f_search_removeCacheAll(object sender, MouseEventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to clear all result search?", "Confirm clear cache search!", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_ONLINE_CACHE_CLEAR });
            }
        }

        private void f_search_saveItemSelected(object sender, MouseEventArgs e)
        {
            if (m_search_item_current_id == 0 || string.IsNullOrEmpty(m_search_item_current_text))
            {
                MessageBox.Show("Please select item from tab search result to save!");
            }
            else
            {
                if (api_media.f_media_Exist(m_search_item_current_id))
                    MessageBox.Show(string.Format("The [{0}] saved", m_search_item_current_text));
                else
                    app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_ONLINE_SAVE_TO_STORE, Input = m_search_item_current_id });
            }
        }

        void f_search_Result(oMediaSearchLocalResult rs)
        {
            f_search_draw_Media(rs.MediaIds);

            int page = rs.CountResult / rs.PageSize;
            if (rs.CountResult % rs.PageSize != 0) page++;

            m_search_PageCurrent.crossThreadPerformSafely(() =>
            {
                m_search_PageCurrent.Text = rs.PageNumber.ToString();
            });
            m_search_PageTotal.crossThreadPerformSafely(() =>
            {
                m_search_PageTotal.Text = page.ToString();
            });
            m_search_TotalItems.crossThreadPerformSafely(() =>
            {
                m_search_TotalItems.Text = rs.CountResult.ToString();
            });
        }

        private void f_search_goPagePrevClick(object sender, EventArgs e)
        {
            if (m_search_current_msg != null)
            {
                if ((m_search_current_msg.PageNumber - 1) * m_search_current_msg.PageSize < m_search_current_msg.Counter)
                {
                    m_search_current_msg.PageNumber = m_search_current_msg.PageNumber + 1;
                    app.postToAPI(m_search_current_msg);
                }
            }
        }

        private void f_search_goPageNextClick(object sender, EventArgs e)
        {
            if (m_search_current_msg != null)
            {
                if (m_search_current_msg.PageNumber > 1)
                {
                    m_search_current_msg.PageNumber = m_search_current_msg.PageNumber - 1;
                    app.postToAPI(m_search_current_msg);
                }
            }
        }

        private void f_search_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string key = m_search_Input.Text.Trim();
                if (key.Length > 1)
                {
                    m_search_Message.Text = "Finding [" + key + "] ...";
                    app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_ONLINE, Input = key, Log = (m_search_caption ? "CC" : string.Empty) });
                }
                else
                    m_search_Message.Text = "Length of keywords must be greater than 1 characters.";
            }
        }

        private void f_search_draw_Media(List<long> ls)
        {
            m_search_Result.crossThreadPerformSafely(() =>
            {
                m_search_Result.Controls.Clear();
            });

            if (ls.Count == 0) return;

            const int margin_bottom = 5;
            const int margin_left = 9;

            int y = 0, x = 0, row = 0;
            Control[] pics = new Control[30];
            Control[] tits = new Control[30];

            #region

            for (int i = 0; i < ls.Count; i++)
            {
                if (i > 29) break;
                if (i == 0 || i == 1)
                {
                    x = i == 0 ? margin_left : (app.m_box_width + margin_left * 2);
                    y = 0;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        row = i / 2;
                        x = margin_left;
                        y = (app.m_item_height * row) + margin_bottom * row;
                    }
                    else
                    {
                        row = (int)(i / 2);
                        x = app.m_box_width + margin_left * 2;
                        y = (app.m_item_height * row) + margin_bottom * row;
                    }
                }

                oMedia media = api_media.f_search_getInfo(ls[i]);
                if (media == null) continue;

                PictureBox pic = new PictureBox()
                {
                    //Text = i.ToString(),
                    //TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.LightGray,
                    Width = app.m_item_width,
                    Height = app.m_item_height,
                    Location = new Point(x, y),
                    Tag = media.Id
                };

                Bitmap img = api_media.f_search_getPhoto(media.Id);
                if (img != null) pic.Image = img;

                Label lbl = new Label()
                {
                    Name = media.Id.ToString(),
                    Text = (i + 1).ToString() + ", " + media.Title,
                    TextAlign = ContentAlignment.MiddleLeft,

                    AutoSize = false,
                    BackColor = Color.LightGray,
                    //ForeColor = Color.Black,
                    Width = app.m_box_width - app.m_item_width,
                    Height = app.m_box_height - app.m_item_height,
                    Location = new Point(pic.Location.X + app.m_item_width, pic.Location.Y),
                    Padding = new Padding(9, 0, 0, 0),
                    Font = font_Title,
                };

                pic.MouseClick += (se, ev) =>
                {
                    ((Control)se).BackColor = Color.Gray;
                    string mid = ((Control)se).Tag.ToString();

                    Control _lbl = m_search_Result.Controls.Find(mid, false).SingleOrDefault();
                    if (_lbl != null)
                        f_search_labelTitle_MouseClick(_lbl, null);
                };
                lbl.MouseClick += f_search_labelTitle_MouseClick;
                lbl.MouseMove += f_form_move_MouseDown;
                pic.MouseMove += f_form_move_MouseDown;

                pics[i] = pic;
                tits[i] = lbl;
            }

            #endregion

            m_search_Result.crossThreadPerformSafely(() =>
            {
                m_search_Result.Controls.AddRange(tits);
                m_search_Result.Controls.AddRange(pics);
            });
        }

        private void f_search_labelTitle_MouseClick(object sender, MouseEventArgs e)
        {
            Control it = ((Control)sender);
            it.BackColor = Color.Orange;
            long mediaId_prev = 0;
            if (m_search_Result.Tag != null) mediaId_prev = (long)m_search_Result.Tag;
            if (mediaId_prev > 0)
            {
                Control itprev = m_search_Result.Controls.Find(mediaId_prev.ToString(), false).SingleOrDefault();
                if (itprev != null)
                    itprev.BackColor = Color.LightGray;
            }

            long mediaId_sel = long.Parse(it.Name);
            m_search_Result.Tag = mediaId_sel;

            if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying) m_media.Ctlcontrols.stop();

            m_search_item_current_id = mediaId_sel;
            m_search_item_current_text = it.Text;
            m_media_current_tab = MEDIA_TAB.TAB_SEARCH;

            m_media_current_id = mediaId_sel;
            m_media_current_title = it.Text;

            m_search_Message.Text = it.Text;
            this.Text = m_media_current_title;
            //lbl_title.Text = m_media_current_title;

            if (e == null)
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_TEXT_VIDEO_ONLINE, Input = mediaId_sel });
        }

        #endregion

        #region [ MEDIA ]

        TextBox m_media_text;

        private void f_media_loadText()
        {
            //app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_TEXT_INFO, Input = m_media_current_id, Log = ((int)m_media_current_tab).ToString() });
            if (m_media_current_id > 0)
            {
                //string text = api_media.f_media_getText(m_media_current_id);
                string[] a = api_media.f_media_getSentences(m_media_current_id);
                m_media_text.Text = Environment.NewLine + Environment.NewLine +
                    string.Join(Environment.NewLine + Environment.NewLine, a);
            }
        }

        private void f_media_loadWord()
        {
            //m_words = api_media.f_media_getWords(m_media_current_id);
            f_word_goPage(1);

            //app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_WORD_LIST, Input = m_media_current_id, Log = ((int)m_media_current_tab).ToString() });
        }

        private void f_media_loadWord_Callback(msg m)
        {
            if (m != null && m.Output != null)
            {
                m_words = m.Output.Data as oWordCount[];
                if (m_words != null && m_words.Length > 0)
                {
                    this.Invoke((Action)(() =>
                    {
                        f_word_goPage(1);
                    }));
                }
            }
        }

        private void f_media_event_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
        }

        void f_video_openMp4_Request()
        {
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_PLAY_VIDEO, Input = m_media_current_id, Log = m_media_current_title });
        }

        void f_video_openMp4_Callback(string url, string title)
        {
            if (url != string.Empty)
            {
                app.f_player_Open(url, title);
            }
            else
                MessageBox.Show("Cannot open videoId: " + title);
        }

        void f_video_openMp3_Request()
        {
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_PLAY_AUDIO, Input = m_media_current_id });
        }

        void f_video_openMp3_Callback(string url, string title)
        {
            if (url != string.Empty)
            {
                this.Invoke((Action)(() =>
                {
                    this.Cursor = Cursors.WaitCursor;
                    btn_play.InActiveColor = Color.Orange;
                    m_media.Visible = true;
                    m_media.URL = url;
                    m_media.close();
                    this.Cursor = Cursors.Default;
                }));
            }
            else
                MessageBox.Show("Cannot open videoId: " + title);

        }

        #endregion

        #region [ PRONUNCE ]

        string m_pronunce_vowel_current = string.Empty;
        string m_pronunce_consonant_current = string.Empty;

        Panel pronunce_Words;
        Panel pronunce_Content;
        Panel pronunce_Footer;
        Label pronunce_Message_Label;

        Panel pronunce_Vowel_Consonant;

        void f_pronunce_initUI()
        {
            var pronunce_Vowel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Left,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown | FlowDirection.LeftToRight,
                Width = app.m_app_width / 2,
                //BackColor = Color.Red,
                Height = 86,
            };
            var pronunce_Consonant = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown | FlowDirection.LeftToRight,
                //BackColor = Color.Blue,
                Height = 99,
            };
            pronunce_Vowel_Consonant = new Panel()
            {
                Dock = DockStyle.Top,
            };
            pronunce_Vowel_Consonant.Controls.AddRange(new Control[] { pronunce_Consonant, pronunce_Vowel });

            pronunce_Words = new Panel()
            {
                Dock = DockStyle.Left,
                BackColor = Color.WhiteSmoke,
                Width = 99,
            };

            pronunce_Content = new Panel()
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = Color.DodgerBlue,
                Font = font_TextView,
            };

            pronunce_Footer = new Panel()
            {
                Height = 25,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Padding = new Padding(109, 0, 9, 0),
            };

            pronunce_Message_Label = new Label()
            {
                Height = 17,
                AutoSize = false,
                Dock = DockStyle.Bottom,
                BackColor = Color.Red,
                TextAlign = ContentAlignment.BottomRight,
            };
            m_tab_Pronunce.Controls.AddRange(new Control[] {
                pronunce_Content,
                pronunce_Words,
                pronunce_Footer,
                pronunce_Message_Label,
                pronunce_Vowel_Consonant,
            });

            TextBox pronunce_word_Input = new TextBox()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Width = m_text_search_width,
                Location = new Point(7, 2),
                Height = 19
            };

            IconButton ico_refresh = new IconButton()
            {
                IconType = IconType.refresh,
                Dock = DockStyle.Right,
            };
            ico_refresh.MouseClick += (se, ev) => { f_browser_fetchURL(); };

            pronunce_Footer.Controls.AddRange(new Control[] {
                pronunce_word_Input,
                ico_refresh,
            });


            string[] av = new string[] { };// api_pronunce.f_get_Vowels();
            string[] ac = new string[] { };// api_pronunce.f_get_Consonants();

            Control[] cv = new Control[av.Length + 1];
            Control[] cc = new Control[ac.Length + 1];

            cv[0] = new Label() { AutoSize = true, Text = "Vowels:", Margin = new Padding(3, 5, 0, 0), };
            cc[0] = new Label() { AutoSize = true, Text = "Consonant:", Margin = new Padding(3, 5, 0, 0), };

            for (int i = 0; i < av.Length; i++)
            {
                cv[i + 1] = new Label()
                {
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = av[i],
                    Name = "lbl_pronunce_" + av[i],
                    Font = font_Title,
                    Height = 17,
                    Padding = new Padding(0),
                    Margin = new Padding(15, 3, 0, 0),
                    //BackColor = Color.Orange,
                };
                cv[i + 1].MouseClick += f_pronunce_vowel_label_Click;
            }
            pronunce_Vowel.Controls.AddRange(cv);


            for (int i = 0; i < ac.Length; i++)
            {
                cc[i + 1] = new Label()
                {
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = ac[i],
                    Name = "lbl_pronunce_" + ac[i],
                    Font = font_Title,
                    Height = 17,
                    Padding = new Padding(0),
                    Margin = new Padding(15, 3, 0, 0),
                    //BackColor = Color.Orange,
                };
                cc[i + 1].MouseClick += f_pronunce_consonant_label_Click;
            }
            pronunce_Consonant.Controls.AddRange(cc);
        }

        private void f_pronunce_consonant_label_Click(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;

            if (!string.IsNullOrEmpty(m_pronunce_consonant_current))
            {
                Control prev = pronunce_Vowel_Consonant.Controls[0].Controls.Cast<Control>().Where(x => x.Name == "lbl_pronunce_" + m_pronunce_consonant_current).SingleOrDefault();
                if (prev != null) prev.BackColor = Color.White;
            }

            if (lbl.BackColor == Color.Orange)
            {
                lbl.BackColor = Color.White;
                m_pronunce_consonant_current = string.Empty;
            }
            else
            {
                lbl.BackColor = Color.Orange;
                m_pronunce_consonant_current = lbl.Text;
                f_pronunce_Filter();
            }
        }

        private void f_pronunce_vowel_label_Click(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;

            if (!string.IsNullOrEmpty(m_pronunce_vowel_current) && lbl.Text != m_pronunce_vowel_current)
            {
                Control prev = pronunce_Vowel_Consonant.Controls[1].Controls.Cast<Control>().Where(x => x.Name == "lbl_pronunce_" + m_pronunce_vowel_current).SingleOrDefault();
                if (prev != null) prev.BackColor = Color.White;
            }

            if (lbl.BackColor == Color.Orange)
            {
                lbl.BackColor = Color.White;
                m_pronunce_vowel_current = string.Empty;
            }
            else
            {
                lbl.BackColor = Color.Orange;
                m_pronunce_vowel_current = lbl.Text;
                f_pronunce_Filter();
            }
        }

        void f_pronunce_Filter()
        {
            if (!string.IsNullOrEmpty(m_pronunce_vowel_current) && lbl_hide_border_left.Text != m_pronunce_consonant_current)
            {
                string url_mp3_vowel = api_pronunce.f_get_PronunceMP3(m_pronunce_vowel_current);
                wd_media.URL = url_mp3_vowel;
            }

            if (!string.IsNullOrEmpty(m_pronunce_consonant_current))
            {
                string url_mp3_Consonant = api_pronunce.f_get_PronunceMP3(m_pronunce_consonant_current);
                wd_media.URL = url_mp3_Consonant;
            }

        }

        #endregion

        #region [ BROWSER ]

        #region

        bool brow_offline_mode = false;
        string brow_offline_url_path = string.Empty;
        string brow_offline_url_current = string.Empty;

        RichTextBoxEx brow_Content;
        Panel brow_Footer;
        TextBox brow_URL_Text;
        Label brow_Message_Label;

        Panel brow_setting;
        IconButton brow_ico_dowload_link;
        IconButton brow_ico_download_stop;
        IconButton brow_ico_download_config;

        static ConcurrentDictionary<string, string> dicHtml = new ConcurrentDictionary<string, string>();

        Panel brow_offline_tools;
        ListBox brow_offline_items;
        Splitter brow_offline_splitter;

        string brow_HTML = string.Empty;
        string brow_URL = string.Empty;
        
        #endregion

        void f_browser_initUI()
        {
            m_tab_Browser.Padding = new Padding(9, 0, 0, 0);

            Panel panel_header = new Panel()
            {
                Height = 25,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 3, 0, 0),
            };

            brow_setting = new Panel()
            {
                Padding = new Padding(20, 7, 20, 7),
                Width = 299,
                Height = 199,
                BackColor = Color.LightGray,
                Location = new Point(app.m_app_width / 2, 99),
                Visible = false,
            };

            var btn_setting_save = new Button() { Text = "SAVE", Dock = DockStyle.Bottom, BackColor = Color.Orange };
            btn_setting_save.MouseClick += f_browser_setting_Save_MouseClick;
            brow_setting.Controls.AddRange(new Control[] {
                btn_setting_save,

                new CheckBox(){ Dock = DockStyle.Top, Tag = "SAVE_RESULT", Text = "Write result to file", Checked = true },
                //new Label(){ Dock = DockStyle.Top, Text = "URL start:", Height = 15,  },
                new Label(){ Dock = DockStyle.Top, Height = 2 },

                new TextBox(){ Dock = DockStyle.Top, Tag = "URL_CONTIANS", Text = test_URL_CONTIANS },
                new Label(){ Dock = DockStyle.Top, Text = "URL start:", Height = 15,  },
                new Label(){ Dock = DockStyle.Top, Height = 3 },

                new TextBox(){ Dock = DockStyle.Top, Tag = "PARA2" },
                new Label(){ Dock = DockStyle.Top, Text = "Para 2:", Height = 15,  },
                new Label(){ Dock = DockStyle.Top, Height = 3 },

                new TextBox(){ Dock = DockStyle.Top, Tag = "PARA1" },
                new Label(){ Dock = DockStyle.Top, Text = "Para 1:", Height = 15 },

                new Label(){ Dock = DockStyle.Top, Height = 3 },
                new Label(){ Dock = DockStyle.Top, Text = "Setting:", Height = 15 },
            });


            brow_Content = new RichTextBoxEx()
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                //BackColor = Color.Yellow,
                Font = font_LogView,
            };
            brow_Footer = new Panel()
            {
                Height = 25,
                Dock = DockStyle.Bottom,
                //BackColor = Color.White,
            };

            brow_URL_Text = new TextBox()
            {
                //Height = 17,
                AutoSize = false,
                Dock = DockStyle.Fill,
                //BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.None,
                Text = test_url,
                Margin = new Padding(0, 9, 0, 0),
                ForeColor = Color.DimGray,
            };
            brow_URL_Text.KeyDown += f_browser_URL_KeyDown;
            brow_URL_Text.MouseDoubleClick += f_browser_offline_OpenFile;

            brow_Message_Label = new Label()
            {
                Height = 17,
                AutoSize = false,
                Dock = DockStyle.Bottom,
                //BackColor = Color.Red,
                TextAlign = ContentAlignment.BottomRight,
            };

            TextBox brow_offline_textSearch = new TextBox()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Width = m_text_search_width,
                Location = new Point(0, 2),
                Height = 19
            };

            IconButton ico_refresh = new IconButton()
            {
                IconType = IconType.refresh,
                Dock = DockStyle.Right,
            };
            ico_refresh.MouseClick += (se, ev) => { f_browser_fetchURL(); };

            IconButton ico_dowload_mp3 = new IconButton()
            {
                IconType = IconType.android_playstore,
                Dock = DockStyle.Right,
                ToolTipText = "Download MP3"
            };
            ico_dowload_mp3.MouseClick += f_browser_download_MP3;

            brow_ico_download_stop = new IconButton(16)
            {
                IconType = IconType.stop,
                Dock = DockStyle.Left,
                ToolTipText = "Stop download",
                Visible = false,
            };
            brow_ico_download_config = new IconButton()
            {
                IconType = IconType.settings,
                Dock = DockStyle.Right,
                ToolTipText = "Setting download"
            };
            brow_ico_download_config.MouseClick += f_browser_download_Setting;

            IconButton ico_dowload_article = new IconButton()
            {
                IconType = IconType.archive,
                Dock = DockStyle.Right,
                ToolTipText = "Download Article"
            };
            ico_dowload_article.MouseClick += f_browser_download_Article;

            brow_ico_dowload_link = new IconButton()
            {
                IconType = IconType.link,
                Dock = DockStyle.Left,
                ToolTipText = "Fetch All URL"
            };

            brow_ico_dowload_link.MouseClick += (se, ev) =>
            {
                var ok = f_browser_download_URL(se, ev);
                if (ok)
                {
                    brow_ico_dowload_link.Visible = false;
                    brow_ico_download_stop.Visible = true;
                    brow_ico_download_config.Visible = false;
                }
            };

            brow_ico_download_stop.MouseClick += (se, ev) =>
            {
                f_browser_download_Stop(se, ev);

                brow_ico_dowload_link.Visible = true;
                brow_ico_download_stop.Visible = false;
                brow_ico_download_config.Visible = true;
            };

            brow_offline_items = new ListBox() {
                Dock = DockStyle.Left,
                Width = 199,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
            };
            brow_offline_splitter = new Splitter() {
                Dock = DockStyle.Left,
                Width = 3,
                BackColor = Color.LightGray,
                MinExtra = 0,
                MinSize = 0,
            };
            brow_offline_items.SelectedIndexChanged += f_browser_offline_articles_selectChanged;
            brow_offline_items.ValueMember = "Item1";
            brow_offline_items.DisplayMember = "Item2";

            IconButton ico_offline_open_on_browser = new IconButton()
            {
                IconType = IconType.android_globe,
                Dock = DockStyle.Left,
                ToolTipText = "Open Article on Browser"
            };
            ico_offline_open_on_browser.MouseClick += f_browser_offline_openArticleOnBrowser;

            brow_offline_tools = new Panel()
            {
                Dock = DockStyle.Left,
                Width = 199,
                //BackColor = Color.Orange,
                Padding = new Padding(109, 0, 9, 0),
                Visible = false,
            };
            
            brow_offline_tools.Controls.AddRange(new Control[] {
                brow_offline_textSearch,
                ico_offline_open_on_browser,
            });
            panel_header.Controls.AddRange(new Control[] {
                brow_URL_Text,
                new Label(){ Dock = DockStyle.Left, Width = 5 },
                brow_ico_download_stop,
                brow_ico_dowload_link,
                brow_ico_download_config,
                //new Label(){ Dock = DockStyle.Right, Width = 5 },
            });
            m_tab_Browser.Controls.AddRange(new Control[] {
                brow_Content,
                brow_offline_splitter,
                brow_offline_items,
                brow_Message_Label,
                brow_Footer,
                panel_header,
                brow_setting,
            });
            brow_Footer.Controls.AddRange(new Control[] {
                brow_offline_tools,
                //brow_word_Input,
                new Label(){ Dock = DockStyle.Right, Width = 5 },
                ico_dowload_article,
                new Label(){ Dock = DockStyle.Right, Width = 5 },
                ico_dowload_mp3,
            });
            brow_setting.BringToFront();
        }

        public static string f_brow_offline_getHTMLByUrl(string url) {
            if (dicHtml.ContainsKey(url))
                return dicHtml[url];
            return string.Empty;
        }

        private void f_browser_offline_openArticleOnBrowser(object sender, MouseEventArgs e)
        {
            if (brow_offline_mode && !string.IsNullOrEmpty(brow_offline_url_current)) {
                string url = api_media.f_proxy_getHost() + "?crawler_key=" + HttpUtility.UrlEncode(brow_offline_url_current);
                System.Diagnostics.Process.Start(url);
            }
        }

        private void f_browser_offline_articles_selectChanged(object sender, EventArgs e)
        {
            if (brow_offline_mode) {
                var it = brow_offline_items.SelectedItem as Tuple<string,string>;
                if (it != null
                    && dicHtml.ContainsKey(it.Item1)) {
                    brow_offline_url_current = it.Item1;
                    f_browser_displayText(dicHtml[brow_offline_url_current]);
                }
            }
        }

        private void f_browser_setting_Save_MouseClick(object sender, MouseEventArgs e)
        {
            brow_setting.SendToBack();
            if (brow_offline_mode) {
                f_browser_offline_analyticHTMLBySetting();
            }
        }

        void f_browser_offline_analyticHTMLBySetting() {
            if (dicHtml.Count > 0) {

            }
        }

        void f_browser_offline_bindArticles() {            
            string name = brow_URL_Text.Text.Trim();
            brow_Message_Label.Text = name + ": have " + dicHtml.Count + " articles";
            f_browser_displayText(string.Empty);

            brow_offline_items.Items.Clear();

            string[] a = dicHtml.Keys.ToArray();
            string url_min = a.Select(x => new oLinkLen { Url = x, Len = x.Length }).MinBy(x => x.Len).Url;
            if (url_min[url_min.Length - 1] != '/') {
                string[] aa = url_min.Split('/');
                url_min = string.Join("/", aa.Where((x, k) => k < aa.Length - 1)) + "/";
            }
            brow_offline_url_path = url_min;

            foreach (string it in a)
            {
                string tit = it.Replace(url_min, string.Empty).Replace('-',' ');
                if(tit.Length > 0)
                    tit = tit[0].ToString().ToUpper() + tit.Substring(1);
                brow_offline_items.Items.Add(new Tuple<string, string>(it, tit));
            }
        }

        void f_browser_displayText(string s)
        {
            brow_Content.Text = s;
            brow_Content.SelectAll();
            brow_Content.SelectionParaSpacing = new RTBParaSpacing(0, 150);
            brow_Content.Select(0, 0);
        }

        void f_browser_offline_OpenFile(object sender, MouseEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "package");
            openFileDialog.Filter = "htm files (*.htm)|*.htm|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                brow_offline_mode = true;
                brow_offline_tools.Visible = true;

                string fi_name = openFileDialog.FileName;
                string name = Path.GetFileName(fi_name);
                brow_URL_Text.Text = name;
                brow_URL_Text.Tag = fi_name;

                using (var file = File.OpenRead(fi_name))
                {
                    dicHtml = Serializer.Deserialize<ConcurrentDictionary<string, string>>(file);
                    file.Close();
                }

                f_browser_offline_bindArticles();
            }
        }

        Dictionary<string, string> f_browser_get_Setting()
        {
            string url = brow_URL_Text.Text.Trim();
            Dictionary<string, string> rs = new Dictionary<string, string>() {
                { "URL", url }
            };
            foreach (Control ti in brow_setting.Controls)
                if (ti is TextBox && ti.Tag != null && !string.IsNullOrEmpty(ti.Text))
                    rs.Add(ti.Tag.ToString(), ti.Text);
                else if (ti is CheckBox && ti.Tag != null)
                    rs.Add(ti.Tag.ToString(), (((CheckBox)ti).Checked ? "1" : "0"));

            return rs;
        }

        bool f_browser_validBeforAction()
        {
            brow_Message_Label.Text = string.Empty;
            string url = brow_URL_Text.Text.Trim();

            if (!url.ToLower().StartsWith("http://") && !url.ToLower().StartsWith("https://"))
            {
                brow_Message_Label.Text = "URL invalid!";
                return false;
            }

            Uri uri;
            if (url.Length > 0 && Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                brow_URL = url;
                var cf = f_browser_get_Setting();
                if (!cf.ContainsKey("URL")) // || !cf.ContainsKey("URL_CONTIANS") )//|| !cf.ContainsKey("PARA1") || !cf.ContainsKey("PARA2"))
                {
                    brow_Message_Label.Text = "Please setting !";
                    f_browser_download_Setting(null, null);
                }
                else
                {
                    brow_offline_tools.Visible = false;
                    brow_Content.Text = string.Empty;
                    brow_offline_mode = true;
                    return true;
                }
            }
            else
            {
                brow_Message_Label.Text = "URL invalid!";
            }
            return false;
        }

        private void f_browser_URL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            }
        }

        private void f_browser_download_Stop(object sender, MouseEventArgs e)
        {
            app.postToAPI(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_STOP });
        }

        private void f_browser_download_Setting(object sender, MouseEventArgs e)
        {
            if (brow_setting.Visible == false)
            {
                brow_setting.Visible = true;
                brow_setting.BringToFront();
            }
            else {
                brow_setting.Visible = false;
            }
        }

        private void f_browser_download_Article(object sender, MouseEventArgs e)
        {
            if (f_browser_validBeforAction())
            {

            }
        }

        private bool f_browser_download_URL(object sender, MouseEventArgs e)
        {
            if (f_browser_validBeforAction())
            {
                string[] auri = brow_URL.Split('/');
                string uri_root = string.Join("/", auri.Where((x, k) => k < 3).ToArray());
                app.postToAPI(new msg()
                {
                    API = _API.CRAWLER,
                    KEY = _API.CRAWLER_KEY_REGISTER_PATH,
                    Input = new oLinkSetting()
                    {
                        Url = brow_URL,
                        Settings = f_browser_get_Setting(),
                    }
                });
                return true;
            }
            return false;
        }

        private void f_browser_download_MP3(object sender, MouseEventArgs e)
        {
            if (f_browser_validBeforAction())
            {

            }
        }

        void f_brow_reset_All()
        {
            brow_HTML = string.Empty;
            brow_URL = string.Empty;

            brow_Message_Label.Text = string.Empty;

            brow_URL_Text.Text = string.Empty;

            brow_Content.Text = string.Empty;
        }

        void f_brow_reset_UI()
        {
            brow_Message_Label.Text = string.Empty;
            brow_URL_Text.Text = string.Empty;
            brow_Content.Text = string.Empty;
        }

        void f_browser_fetchURL()
        {
            if (brow_Content == null) return;

            List<string> urls = new List<string>();

            brow_Message_Label.Text = "Syncing with chrome tab current...";

            // there are always multiple chrome processes, so we have to loop through all of them to find the
            // process with a Window Handle and an automation element of name "Address and search bar"
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }

                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);
                AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));

                // if it can be found, get the value from the URL bar
                if (elmUrlBar != null)
                {
                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        //Console.WriteLine("Chrome URL found: " + val.Current.Value);
                        if (val != null
                            && !string.IsNullOrEmpty(val.Current.Value)
                            && brow_URL != val.Current.Value)
                        {
                            string url = val.Current.Value;
                            if (url.StartsWith("http"))
                                urls.Add(url);
                        }
                    }
                }
            }

            if (urls.Count > 0)
            {
                string url = urls[0];
                if (brow_URL == url)
                {
                    //f_brow_reset_All();
                    f_brow_analytic_HTML();
                }
                else
                {
                    brow_URL = url;

                    f_brow_reset_UI();

                    brow_URL_Text.Text = brow_URL;

                    using (WebClient w = new WebClient())
                    {
                        w.Encoding = Encoding.UTF8;
                        brow_HTML = w.DownloadString(brow_URL);
                    }

                    f_brow_analytic_HTML();
                }
            }
            else
            {
                f_brow_reset_All();
                brow_Message_Label.Text = string.Empty;
            }
        }

        void f_brow_analytic_HTML()
        {
            string s = api_media.f_article_analytic_HTML(brow_URL, brow_HTML);
            f_browser_displayText(s);
        }


        void f_brow_update_UI_fromResponseAPI(msg m)
        {

            switch (m.KEY)
            {
                case _API.CRAWLER_KEY_REQUEST_LINK:
                    brow_Message_Label.crossThreadPerformSafely(() =>
                    {
                        brow_Message_Label.Text = m.Log;
                    });
                    break;
                case _API.CRAWLER_KEY_REQUEST_LINK_COMPLETE:
                    this.Invoke((Action)(() =>
                    {
                        brow_ico_dowload_link.Visible = true;
                        brow_ico_download_stop.Visible = false;
                        brow_ico_download_config.Visible = true;
                        brow_Content.Text = string.Join(Environment.NewLine, m.Input as string[]);
                    }));
                    break;
            }

        }
        #endregion

        #region [ FORM MOVE ]

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void f_form_move_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region [ RESPONSE MESSAGE ]

        public void api_responseMsg(object sender, threadMsgEventArgs e)
        {
            msg m = e.Message;
            if (m != null)
            {
                switch (m.API)
                {
                    #region [ MSG ]
                    case _API.MSG_MEDIA_SEARCH_RESULT:
                        log.Append(m.Log + Environment.NewLine);
                        m_search_Message.crossThreadPerformSafely(() =>
                        {
                            m_search_Message.Text = m.Log;
                        });
                        m_search_Message.crossThreadPerformSafely(() =>
                        {
                            m_search_Message.Text = m.Log;
                        });
                        app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_ONLINE_CACHE, Input = string.Empty });
                        break;
                    case _API.MSG_MEDIA_SEARCH_SAVE_TO_STORE:
                        log.Append(m.Log + Environment.NewLine);
                        m_search_Message.crossThreadPerformSafely(() =>
                        {
                            m_search_Message.Text = m.Log;
                        });
                        app.postToAPI(m_search_current_msg);
                        break;
                    #endregion
                    case _API.WORD:
                        #region
                        switch (m.KEY)
                        {
                            case _API.WORD_KEY_ANALYTIC:
                                if (m.Output.Ok && (long)m.Input == m_media_current_id)
                                {
                                    m_media_text.crossThreadPerformSafely(() =>
                                    {
                                        m_media_text.Text = (string)m.Output.Data;
                                    });
                                }
                                break;
                        }
                        break;
                    #endregion
                    case _API.MEDIA:
                        #region
                        switch (m.KEY)
                        {
                            case _API.MEDIA_KEY_WORD_TRANSLATER:
                                word_Translater.crossThreadPerformSafely(() =>
                                {
                                    word_Translater.InActiveColor = Color.DimGray;
                                });
                                MessageBox.Show(m.Log);
                                break;
                            case _API.MEDIA_KEY_UPDATE_BOOKMARK_STAR:
                                #region
                                if (!string.IsNullOrEmpty(m.Log))
                                {
                                    log.Append(m.Log + Environment.NewLine);
                                    m_store_Message.crossThreadPerformSafely(() =>
                                    {
                                        m_store_Message.Text = m.Log;
                                    });
                                }
                                break;
                            #endregion

                            #region [ SEARCH ONLINE ]

                            case _API.MEDIA_KEY_SEARCH_ONLINE_CACHE_CLEAR:
                                log.Append(m.Log + Environment.NewLine);
                                m_store_Message.crossThreadPerformSafely(() =>
                                {
                                    m_store_Message.Text = m.Log;
                                });
                                break;
                            case _API.MEDIA_KEY_TEXT_VIDEO_ONLINE:
                                if (m.Output.Ok)
                                {
                                    m_media_text.crossThreadPerformSafely(() =>
                                    {
                                        m_media_text.Text = (string)m.Output.Data;
                                    });
                                }
                                break;
                            case _API.MEDIA_KEY_PLAY_VIDEO_ONLINE:
                                if (m.Output.Ok)
                                {
                                    f_video_openMp4_Callback((string)m.Output.Data, m.Log);
                                    this.Invoke((Action)(() =>
                                    {
                                        btn_play.Visible = false;
                                        m_media.Visible = false;
                                        this.Text = "English";
                                        this.lbl_title.Text = string.Empty;
                                    }));
                                }
                                break;
                            case _API.MEDIA_KEY_SEARCH_ONLINE_CACHE:
                                if (m.Output.Ok)
                                {
                                    var rs = (oMediaSearchLocalResult)m.Output.Data;
                                    f_search_Result(rs);
                                    m_search_current_msg = m.clone(m.Input);
                                }
                                else
                                {
                                    MessageBox.Show("Search online error");
                                }
                                break;

                            #endregion

                            case _API.MEDIA_KEY_FILTER_BOOKMAR_STAR:
                            case _API.MEDIA_KEY_SEARCH_STORE:
                                if (m.Output.Ok)
                                {
                                    var rs = (oMediaSearchLocalResult)m.Output.Data;
                                    f_store_Result(rs);
                                    m_store_current_msg = m.clone(m.Input);
                                }
                                else
                                {
                                    MessageBox.Show("Search store error");
                                }
                                break;
                            case _API.MEDIA_KEY_PLAY_AUDIO:
                                if (m.Output.Ok && (long)m.Input == m_media_current_id)
                                    f_video_openMp3_Callback((string)m.Output.Data, m.Log);
                                break;
                            case _API.MEDIA_KEY_PLAY_VIDEO:
                                if (m.Output.Ok && (long)m.Input == m_media_current_id)
                                    f_video_openMp4_Callback((string)m.Output.Data, m.Log);
                                break;
                            case _API.MEDIA_KEY_TEXT_INFO:
                                if (m.Output.Ok)
                                {
                                    m_media_text.crossThreadPerformSafely(() =>
                                    {
                                        m_media_text.Text = (string)m.Output.Data;
                                    });
                                }
                                break;
                            case _API.MEDIA_KEY_WORD_LIST:
                                f_media_loadWord_Callback(m);
                                break;
                        }
                        break;
                    #endregion
                    case _API.CRAWLER:
                        f_brow_update_UI_fromResponseAPI(m);
                        break;
                }
            }
        }

        public void f_form_freeResource()
        {
        }

        #endregion

        public fMain()
        {
            log = new StringBuilder();
            this.Icon = Resources.favicon;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Shown += (se, ev) => f_main_Shown();

            f_audio_initUI();
            f_tab_initUI();

            f_store_initUI();
            f_search_initUI();

            f_word_detail_Init();
            f_word_Init();
            f_browser_initUI();

            f_pronunce_initUI();
        }

    }
}
