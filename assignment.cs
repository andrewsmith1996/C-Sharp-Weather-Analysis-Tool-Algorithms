using System;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

class main{
  public static void Main(string[] args){

    //Creates objects to call functions and methods from
    menuSystem menuObject = new menuSystem();
    textFiles textFilesObject = new textFiles();
    sort algorithmObject = new sort();
    output outputObject = new output();

    //Uses the menu class to get the input options from the user
    string weatherStation = menuObject.getWeatherStation();
    string whatToView = menuObject.getWhatToView();
    int menuChoiceTime = menuObject.getWhatTime();
    int menuChoiceDo = menuObject.getWhatToDo();
    int menuChoiceOutput = menuObject.getOutputType();

    //Initialise  variables
    string year = "";
    string month = "";

    //Reads in the year and months
    string[] yearLines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/Year.txt");
    string[] monthLines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/Month.txt");

    //Gets the year and month from the user and validates it
    if(menuChoiceTime == 1){
         year = menuObject.getYear(yearLines);
    } else if(menuChoiceTime == 2){
         year = menuObject.getYear(yearLines);
         month = menuObject.getMonth(year, yearLines, monthLines, menuChoiceTime);
    } else if(menuChoiceTime == 4){
         month = menuObject.getMonth(year, yearLines, monthLines, menuChoiceTime);
    }

    //Assigns an array the correct text file based on what the user wants to view
    double[] data = textFilesObject.loadInData(weatherStation, whatToView);

    //Initliases variables
    int menuAlgorithmChoice;
    object[] returnArrays;

    int stepCounter;

    //Switch for doing what the user has specified
    switch(menuChoiceDo){
          //If user chosen to sort
          case 1: case 2:
            //Get which sorting algorithm the user wants to use
            menuAlgorithmChoice = menuObject.getSortingAlgorithm();

          //Selects what algorithm to use via a nested switch statement
          switch(menuAlgorithmChoice){
                case 1: //Use Insertion Sort
                      algorithmObject.InsertionSort(data, yearLines, monthLines, menuChoiceTime, month, year, menuChoiceDo, menuChoiceOutput);
                break;

                case 2: //Use QuickSort

                       //Initialise the step counter
                       stepCounter = 0;

                       returnArrays = algorithmObject.Quicksort(data, 0, data.Length - 1, yearLines, monthLines, menuChoiceTime, month, year, menuChoiceDo, stepCounter);

                       //Retrieve the arrays from the object array created by the quicksort function
                       data = (double[])returnArrays[0];
                       yearLines = (string[])returnArrays[1];
                       monthLines = (string[])returnArrays[2];
                       stepCounter = (int)returnArrays[3];

                       //If the user wants to view the data backwards then reverse the array
                       if(menuChoiceDo == 2){
                         Array.Reverse(data);
                         Array.Reverse(monthLines);
                         Array.Reverse(yearLines);
                       }

                       //output the steps the algorithm took
                       Console.WriteLine("This Algorithm took {0} steps", stepCounter);

                       //Call the output method
                       outputObject.doOutput(menuChoiceOutput, data, menuChoiceTime, year, month, yearLines, monthLines);

                  break;

                case 3: //Use Bubblesort
                      algorithmObject.Bubblesort(data, yearLines, monthLines, menuChoiceTime, month, year, menuChoiceDo, menuChoiceOutput);
                break;

                //default class
                default:
                  Console.WriteLine("Error");
                break;
              }

      break; //End of outside case 1 and 2
        case 3: //Find the Median value

            //Initialise the step counter
            stepCounter = 0;

            //Gets the sorted data
            returnArrays = algorithmObject.Quicksort(data, 0, data.Length - 1, yearLines, monthLines, menuChoiceTime, month, year, menuChoiceDo, stepCounter);

            //Retrieves the data from the object as needed to return 3 vairbales
            data = (double[])returnArrays[0];
            yearLines = (string[])returnArrays[1];
            monthLines = (string[])returnArrays[2];
            stepCounter = (int)returnArrays[3];

            //output the steps the algorithm took
            Console.WriteLine("This Algorithm took {0} steps", stepCounter);

            //Call the output method to output the median values
            outputObject.doOutputMedian(menuChoiceOutput, data, menuChoiceTime, year, month, yearLines, monthLines);

        break;

        //Max and Min
        case 4:

            //Initialise the step counter
            stepCounter = 0;

            //Retrieve sorted array
            returnArrays = algorithmObject.Quicksort(data, 0, data.Length - 1, yearLines, monthLines, menuChoiceTime, month, year, menuChoiceDo, stepCounter);

            //Retrieves and assigns individual lists
            data = (double[])returnArrays[0];
            yearLines = (string[])returnArrays[1];
            monthLines = (string[])returnArrays[2];
            stepCounter = (int)returnArrays[3];

            //output the steps the algorithm took
            Console.WriteLine("This Algorithm took {0} steps", stepCounter);

            //Call the output method to output the data
            outputObject.doOutputMinMax(menuChoiceOutput, data, menuChoiceTime, year, month, yearLines, monthLines);

        break;

        case 5: //Sort Chronoligcally
            //Calls the output method to output the data
            outputObject.doOutput(menuChoiceOutput, data, menuChoiceTime, year, month, yearLines, monthLines);
        break;

        case 6: //Sort Chronoligcally Backwards

          //Reveres the arrays to display them from old to new
          Array.Reverse(data);
          Array.Reverse(yearLines);
          Array.Reverse(monthLines);

          //use the output class and the user selected output to output the data
          outputObject.doOutput(menuChoiceOutput, data, menuChoiceTime, year, month, yearLines, monthLines);
        break;

      default:
        Console.WriteLine("Error");
        break;
      }

    }
}

class output{
    public string nameFile(){
        //Retrieves and returns the filename that the user has entered
        Console.WriteLine("Name your file");
        string fileName = Console.ReadLine();

        return fileName;
    }

    public void doOutput(int menuChoiceOutput, double[] data, int menuChoiceTime, string year, string month, string[] yearLines, string[] monthLines){
      //Creates the object to use
      output outputObject = new output();

      //Display the data based on the user input earlier
      if(menuChoiceOutput != 1){
          string text = "";
          //Get filename from user
          string fileName = outputObject.nameFile();

          if(menuChoiceOutput == 2){ //If searching for year
            for(int x = 0; x < data.Length; x++){
                if(menuChoiceTime == 1){
                    if(yearLines[x] == year){
                      text += yearLines[x] + " " + monthLines[x] + " " + data[x];
                      text += "\n";
                    }
                } else if(menuChoiceTime == 2){ //If searching for year and month
                    if(monthLines[x] == month  && yearLines[x] == year){
                      text += yearLines[x] + " " + monthLines[x] + " " + data[x];
                      text += "\n";
                    }
                } else if (menuChoiceTime == 3){ //If searching for all
                    text += yearLines[x] + " " + monthLines[x] + " " + data[x];
                    text += "\n";
                } else if (menuChoiceTime == 4){
                  if(monthLines[x] == month){
                    text += yearLines[x] + " " + monthLines[x] + " " + data[x];
                    text += "\n";
                  }
                }
            }

             //Write the variables to the file
             File.WriteAllText(fileName + ".txt", text);
             Console.WriteLine("Done");

          } else if(menuChoiceOutput == 3){
                for(int x = 0; x < data.Length; x++){
                  if(menuChoiceTime == 1){
                      if(yearLines[x] == year){
                        text += yearLines[x] + " " + monthLines[x] + " " + data[x] ;
                        text += "<br>";
                      }
                  } else if(menuChoiceTime == 2){
                      if(monthLines[x] == month  && yearLines[x] == year){
                        text += yearLines[x] + " " + monthLines[x] + " " + data[x];
                        text += "<br>";
                      }
                  } else if (menuChoiceTime == 3){
                      text += yearLines[x] + " " + monthLines[x] + " " + data[x];
                      text += "<br>";
                  } else if (menuChoiceTime == 4){
                    if(monthLines[x] == month){
                      text += yearLines[x] + " " + monthLines[x] + " " + data[x];
                      text += "<br>";
                    }
                }
              }

            //Write the variable to the html file
            File.WriteAllText(fileName + ".html", text);
            Console.WriteLine("Done");

          }
      } else {
            //Display the text normally to the console
            for(int x = 0; x < data.Length; x++){
                if(menuChoiceTime == 1){
                    if(yearLines[x] == year){
                      Console.WriteLine(yearLines[x] + " " + monthLines[x] + " " + data[x]);
                    }
                } else if(menuChoiceTime == 2){
                    if(monthLines[x] == month  && yearLines[x] == year){
                      Console.WriteLine(yearLines[x] + " " + monthLines[x] + " " + data[x]);
                    }
                } else if (menuChoiceTime == 3){
                    Console.WriteLine(yearLines[x] + " " + monthLines[x] + " " + data[x]);
                } else if (menuChoiceTime == 4){
                    if(monthLines[x] == month){
                      Console.WriteLine(yearLines[x] + " " + monthLines[x] + " " + data[x]);
                    }
                }
            }

    }
  }


    public void doOutputMedian(int menuChoiceOutput, double[] data, int menuChoiceTime, string year, string month, string[] yearLines, string[] monthLines){
      //Creates output object to access the methods of the class
      output outputObject = new output();

      //Finds the midpoint of the list to find the middle element
      int midPoint = data.Length / 2;

      //Initalise text variable
      string text = "";
      string fileName = "";

      //Display the correct data
      if(menuChoiceOutput != 1){
        //Get the user input for the filename
          fileName = outputObject.nameFile();

          //Create the text to add to files
          if(menuChoiceOutput == 2){
              if(menuChoiceTime == 1){
                text = "The Median Value for " + year + " is " + data[midPoint];
              } else if(menuChoiceTime == 2){
                text = "The Median Value for " + month + " " + year + " is " + data[midPoint];
              } else if (menuChoiceTime == 3){
                text = "The Median Value for all data is " + data[midPoint];
              } else if(menuChoiceTime == 4){
                text = "The Median Value for " + month + " is " + data[midPoint];
              }
            }

             //Writes to text file
             File.WriteAllText(fileName + ".txt", text);
             Console.WriteLine("Done");

          } else if(menuChoiceOutput == 3){
              if(menuChoiceTime == 1){
                text = "The Median Value for " + year + " is " + data[midPoint];
              } else if(menuChoiceTime == 2){
                text = "The Median Value for " + month + " " + year + " is " + data[midPoint];
              } else if (menuChoiceTime == 3){
                text = "The Median Value for all data is " + data[midPoint];
              } else if(menuChoiceTime == 4){
                text = "The Median Value for " + month + " is " + data[midPoint];
              }

            //Writes to HTML file
            File.WriteAllText(fileName + ".html", text);
            Console.WriteLine("Done");

      } else{
        //Displays the data to the console
        if(menuChoiceTime == 1){
          Console.WriteLine("The Median Value for {0} is {1}", year, data[midPoint]);
        } else if(menuChoiceTime == 2){
          Console.WriteLine("The Median Value for {0} {1} is {2}", month, year, data[midPoint]);
        } else if (menuChoiceTime == 3){
          Console.WriteLine("The Median Value for all data is {0}", data[midPoint]);
        } else {
          Console.WriteLine("The Median Value for {0} is {1}", month, data[midPoint]);
        }
      }
    }


    public void doOutputMinMax(int menuChoiceOutput, double[] data, int menuChoiceTime, string year, string month, string[] yearLines, string[] monthLines){

      //Initalise the min and max value positions
      int min = 0;
      int max = data.Length - 1;
      output outputObject = new output();
      //Display the correct data
      if(menuChoiceOutput != 1){
          string text = "";
          string fileName = outputObject.nameFile();

          if(menuChoiceOutput == 2){
              if(menuChoiceTime == 1){
                  text +=  year + " " + " Min: " + data[min] + " Max: " + data[max];
              } else if(menuChoiceTime == 2){
                  text += year + " " + month + " Min: " + data[min] + "Max: " + data[max];
              } else if (menuChoiceTime == 3){
                  text += "All Data Min: " + data[min] + " " + yearLines[min] + " " + monthLines[min] + " Max: " + data[max] + " " + yearLines[max] + " " + monthLines[max];
              } else{
                text += month + " " + " Min: " + data[min] + " Max: " + data[max];
              }

             // Writes to text file
             File.WriteAllText(fileName + ".txt", text);
             Console.WriteLine("Done");

          } else if(menuChoiceOutput == 3){
              if(menuChoiceTime == 1){
                  text += year + " Min: " + data[min] + " Max: " + data[max];
              } else if(menuChoiceTime == 2){
                  text += year + " " + month + " Min: " + data[min] + " " + "Max: " + data[max];
                } else if (menuChoiceTime == 3){
                    text += "All Data Min: " + data[min] + " " + yearLines[min] + " " + monthLines[min] + " Max: " + data[max] + " " + yearLines[max] + " " + monthLines[max];
                } else{
                  text += month + " " + " Min: " + data[min] + " Max: " + data[max];
                }

            //Writes to html file
            File.WriteAllText(fileName + ".html", text);
            Console.WriteLine("Done");
          }
      } else {
          if(menuChoiceTime == 1){
              Console.WriteLine("{0} Min: {1} Max: {2}", year, data[min], data[max]);
          } else if(menuChoiceTime == 2){
              Console.WriteLine("{0} {1} Min: {2} Max: {3}", year, month, data[min], data[max]);
          } else if(menuChoiceTime == 3){
              Console.WriteLine("All Data Min: {0} {1} {2} Max: {3} {4} {5}", data[min], yearLines[min], monthLines[min], data[max], yearLines[max], monthLines[max]);
          } else{
            Console.WriteLine("{0} Min: {1} Max: {2}", month, data[min], data[max]);
          }
        }
    }

}

class menuSystem{
    public string getWeatherStation(){
      Console.Clear();
      Console.WriteLine("Weather Analysis");
      Console.WriteLine("Select Weather Station");
      Console.WriteLine("----------------");
      Console.WriteLine("1 - Lerwick");
      Console.WriteLine("2 - Ross-On-Wyke");
      Console.WriteLine("----------------");


      //Validation of weather station choice
      int menuChoiceWeatherStation = 0;

      while(true){
         menuChoiceWeatherStation = Convert.ToInt16(Console.ReadLine());
         if(menuChoiceWeatherStation == 1 || menuChoiceWeatherStation == 2){
           break;
         } else{
           Console.WriteLine("Error - Invalid Input");
         }
      }

      //Matches the weather station choice to the correct text version of the weather station
      string weatherStation = "";

      switch(menuChoiceWeatherStation){
        case 1:
          weatherStation = "Lerwick";
          break;
        case 2:
          weatherStation = "Ross-On-Wyke";
          break;
        default:
          Console.WriteLine("Error");
          break;
      }

    return weatherStation;
    }

    public string getWhatToView(){
      Console.Clear();
      Console.WriteLine("Weather Analysis");
      Console.WriteLine("Select what to view");
      Console.WriteLine("----------------");
      Console.WriteLine("1 - Mean Daily Max Temperature");
      Console.WriteLine("2 - Mean Daily Min Temperature");
      Console.WriteLine("3 - Days of Air Frost");
      Console.WriteLine("4 - Total mm of Rainfall");
      Console.WriteLine("5 - Total Hours of Sun");
      Console.WriteLine("----------------");

      //Validation of this choice
      int menuChoiceView = 0;

      while(true){
         menuChoiceView = Convert.ToInt16(Console.ReadLine());
         if(menuChoiceView == 1 || menuChoiceView == 2 || menuChoiceView == 3 || menuChoiceView == 4 || menuChoiceView == 5){
           break;
         } else{
           Console.WriteLine("Error - Invalid Input");
         }
      }

      //Matches this to text versions
      string whatToView = "";

      switch(menuChoiceView){
        case 1:
          whatToView = "meanMaxTemp";
          break;
        case 2:
          whatToView = "meanMinTemp";
          break;
        case 3:
          whatToView = "daysAirFrost";
          break;
        case 4:
          whatToView = "daysRainfall";
          break;
        case 5:
          whatToView = "daysSun";
          break;
        default:
          Console.WriteLine("Error");
          break;
      }

    return whatToView;
    }

    public int getWhatTime(){
      //Menu system for selecting the time
      Console.Clear();
      Console.WriteLine("Weather Analysis");
      Console.WriteLine("Select Time");
      Console.WriteLine("----------------");
      Console.WriteLine("1 - Search for Year");
      Console.WriteLine("2 - Search Month and Year");
      Console.WriteLine("3 - View All");
      Console.WriteLine("4 - View a Month across Years");
      Console.WriteLine("----------------");

      //Validates the choice
      int menuChoiceTime = 0;

      while(true){
         menuChoiceTime = Convert.ToInt16(Console.ReadLine());
         if(menuChoiceTime == 1 || menuChoiceTime == 2 || menuChoiceTime == 3 || menuChoiceTime == 4){
           break;
         } else{
           Console.WriteLine("Error - Invalid Input");
         }
      }

    return menuChoiceTime;
    }

    public string getYear(string[] yearLines){
      string year = "";
      while(true){
         Console.WriteLine("Enter Year");
         year = Console.ReadLine().ToUpper();
         if(yearLines.Contains(year)){
           break;
         } else{
           Console.WriteLine("Error - Invalid Input");
         }
      }

    return year;
    }

    public string getMonth(string year, string[] yearLines, string[] monthLines, int menuChoiceTime){
      //Get the culture property of the thread.
      CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

      //Create TextInfo object.
      TextInfo textInfo = cultureInfo.TextInfo;

      string month = "";

      while(true){
        Console.WriteLine("Enter Month");
        month = Console.ReadLine();
        month = textInfo.ToTitleCase(month);

        if(menuChoiceTime == 4){
          if(monthLines.Contains(month)){
            break;
          } else{
            Console.WriteLine("Error - Invalid Input");
          }
        } else {
          if(yearLines.Contains(year) && monthLines.Contains(month)){
            break;
          } else{
            Console.WriteLine("Error - Invalid Input");
          }
        }
      }

    return month;
    }

    public int getWhatToDo(){
      //Menu on how to sort the data
      Console.Clear();
      Console.WriteLine("Weather Analysis");
      Console.WriteLine("Select what to do");
      Console.WriteLine("----------------");
      Console.WriteLine("1 - Sort Descending (Low to High)");
      Console.WriteLine("2 - Sort Ascending (High to Low)");
      Console.WriteLine("3 - View Median Values");
      Console.WriteLine("4 - View Max and Min Values");
      Console.WriteLine("5 - Sort Chronologically");
      Console.WriteLine("6 - Sort Chronologically (Backwards)");
      Console.WriteLine("----------------");

      //Validates this choice
      int menuChoiceDo = 0;

      while(true){
         menuChoiceDo = Convert.ToInt16(Console.ReadLine());
         if(menuChoiceDo == 1 || menuChoiceDo == 2 || menuChoiceDo == 3 || menuChoiceDo == 4 || menuChoiceDo == 5 || menuChoiceDo == 6){
           break;
         } else{
           Console.WriteLine("Error - Invalid Input");
         }
      }

    return menuChoiceDo;
    }

    public int getSortingAlgorithm(){

      //Menu system for selecting the time
      Console.Clear();
      Console.WriteLine("Weather Analysis");
      Console.WriteLine("Select Sorting Algorithm");
      Console.WriteLine("----------------");
      Console.WriteLine("1 - Insertion Sort O(n^2)");
      Console.WriteLine("2 - Quicksort O(nlogN)");
      Console.WriteLine("3 - Bubble Sort O(n^2)");
      Console.WriteLine("----------------");

      //Validates the choice
      int menuAlgorithmChoice = 0;

      while(true){
         menuAlgorithmChoice = Convert.ToInt16(Console.ReadLine());
         if(menuAlgorithmChoice == 1 || menuAlgorithmChoice == 2 || menuAlgorithmChoice == 3){
           break;
         } else{
           Console.WriteLine("Error - Invalid Input");
         }
      }

    return menuAlgorithmChoice;
    }

    public int getOutputType(){
      //Menu system for selecting the time
      Console.Clear();
      Console.WriteLine("Weather Analysis");
      Console.WriteLine("Select how to output");
      Console.WriteLine("----------------");
      Console.WriteLine("1 - To Console");
      Console.WriteLine("2 - To Text File (.txt)");
      Console.WriteLine("3 - To Webpage (.html)");
      Console.WriteLine("----------------");

      //Validates the choice
      int menuChoiceOutput = 0;

      while(true){
         menuChoiceOutput = Convert.ToInt16(Console.ReadLine());
         if(menuChoiceOutput == 1 || menuChoiceOutput == 2 || menuChoiceOutput == 3){
           break;
         } else{
           Console.WriteLine("Error - Invalid Input");
         }
      }

    return menuChoiceOutput;
    }

}

class sort{

  public object[] removeData(int menuChoiceTime, double[] data, string[] yearLines, string[] monthLines, string year, string month){

      //Lists for removing data
      List<double> dataList = new List<double>();
      List<string> yearList = new List<string>();
      List<string> monthList = new List<string>();

      //Algorithms for removing unnecessary items
      //If searching for year, add only the correct data to the new array
      if(menuChoiceTime == 1){
          for(int x = 0; x != data.Length; x++){
              if(yearLines[x] == year){
                dataList.Add(data[x]);
                yearList.Add(yearLines[x]);
                monthList.Add(monthLines[x]);
              }
          }

      //If year and month
      } else if(menuChoiceTime == 2){
            for(int x = 0; x != data.Length; x++){
                if(yearLines[x] == year && monthLines[x] == month){ //If the year and month matches the specified ones
                  dataList.Add(data[x]);
                  yearList.Add(yearLines[x]);
                  monthList.Add(monthLines[x]);
                }
              }
        } else if(menuChoiceTime == 4){
            for(int x = 0; x != data.Length; x++){
                if(monthLines[x] == month){ //If the month matches the specified ones
                  dataList.Add(data[x]);
                  yearList.Add(yearLines[x]);
                  monthList.Add(monthLines[x]);
                }
            }
        }

      //Turn this list back into a standard array
      data = dataList.ToArray();
      monthLines = monthList.ToArray();
      yearLines = yearList.ToArray();

      //Creates an array of 3 elements so that all 3 arrays can be returned from the function
      object[] returnArrays2 = new object[3];

      //Assign each position of the array the correct data
      returnArrays2[0] = data;
      returnArrays2[1] = yearLines;
      returnArrays2[2] = monthLines;

      return returnArrays2;

    }

    public void InsertionSort(double[] data, string[] yearLines, string[] monthLines, int menuChoiceTime, string month, string year, int menuChoiceDo, int menuChoiceOutput){
         //Initalise the count variables
         int numSorted = 1;
         int index;
         int stepCounter = 0;

         //Initialise object
         output outputObject = new output();

         while (numSorted < data.Length){
            //Assign the temporary data
            double temp = data[numSorted];
            string tmpYear = yearLines[numSorted];
            string tmpMonth = monthLines[numSorted];
            //Do the swapping
            for (index = numSorted; index > 0; index--){
                   if (temp < data[index-1]){
                         data[index] = data[index-1];
                         yearLines[index] = yearLines[index - 1];
                         monthLines[index] = monthLines[index - 1];
                         stepCounter++;
                   } else {
                         break;
                   }
            }
            //Swap the temp elements back in
            data[index] = temp;
            yearLines[index] = tmpYear;
            monthLines[index] = tmpMonth;
            numSorted++;
          }

          //Reverse the array if viewing descending
          if(menuChoiceDo == 2){
            Array.Reverse(data);
            Array.Reverse(monthLines);
            Array.Reverse(yearLines);
          }

          Console.WriteLine("------------------------------------------");
          Console.WriteLine("This Algorithm took {0} steps", stepCounter);
          Console.WriteLine("------------------------------------------");

          //Call the output method to output the data
          outputObject.doOutput(menuChoiceOutput, data, menuChoiceTime, year, month, yearLines, monthLines);


    }

    public object[] Quicksort(double[] data, int left, int right, string[] yearLines, string[] monthLines, int menuChoiceTime, string month, string year, int menuChoiceDo, int stepCounter){
      //Initialise the variables
      int i = left, j = right;
      double pivot = data[(left + right) / 2];


      //Compare the elements
      while (i <= j){
          while (data[i].CompareTo(pivot) < 0){
              i++;
          }

          while (data[j].CompareTo(pivot) > 0){
              j--;
          }

          //Do the swapping with the temp variables
          if (i <= j){
              double tmp = data[i];
              data[i] = data[j];
              data[j] = tmp;

              string tmpyear = yearLines[i];
              yearLines[i] = yearLines[j];
              yearLines[j] = tmpyear;

              string tmpmonth = monthLines[i];
              monthLines[i] = monthLines[j];
              monthLines[j] = tmpmonth;

              //Increase the counts
              i++;
              j--;
              stepCounter++;
          }
      }

      // Recursive call to carry on sorting
      if (left < j){
          Quicksort(data, left, j, yearLines, monthLines, menuChoiceTime, month, year, menuChoiceDo, stepCounter);
      }

      if (i < right){
          Quicksort(data, i, right, yearLines, monthLines, menuChoiceTime, month, year, menuChoiceDo, stepCounter);
      }



      //Creates an array of 3 elements so that all 3 arrays can be returned from the function
      object[] returnArrays = new object[4];

      //Assign each position of the array the correct data
      returnArrays[0] = data;
      returnArrays[1] = yearLines;
      returnArrays[2] = monthLines;
      returnArrays[3] = stepCounter;

      //Return the object array containing the 3 arrays
      return returnArrays;

      }


    public void Bubblesort(double[] data, string[] yearLines, string[] monthLines, int menuChoiceTime, string month, string year, int menuChoiceDo, int menuChoiceOutput){

    //Initalise the temporary variables
    double temp = 0;
    string tmpYear = "";
    string tmpMonth = "";
    int stepCounter = 0;

    //Initialise object
    output outputObject = new output();

    //Do swapping
    //Sorts 3 lists at once based upon the comparison
    for (int write = 0; write < data.Length; write++) {
        for (int sort = 0; sort < data.Length - 1; sort++) {
            if (data[sort] > data[sort + 1]) {
                //Assign the temporary variables
                temp = data[sort + 1];
                tmpYear = yearLines[sort + 1];
                tmpMonth = monthLines[sort  + 1];

                //Swap the data
                data[sort + 1] = data[sort];
                yearLines[sort + 1] = yearLines[sort];
                monthLines[sort + 1] = monthLines[sort];

                //Swap the temp variables back in
                data[sort] = temp;
                yearLines[sort] = tmpYear;
                monthLines[sort] = tmpMonth;

                //Increases the step counter
                stepCounter++;
            }
        }
    }

    //Reverse the arrays if viewing as descending
    if(menuChoiceDo == 2){
      Array.Reverse(data);
      Array.Reverse(monthLines);
      Array.Reverse(yearLines);
    }

    //Output the steps the algorithm took
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("This Algorithm took {0} steps", stepCounter);
    Console.WriteLine("------------------------------------------");

    //Calls the output class and the output method to output the data
    outputObject.doOutput(menuChoiceOutput, data, menuChoiceTime, year, month, yearLines, monthLines);


  }
}

class textFiles{
  public double[] loadInData(string weatherStation, string whatToView){
  //Initialises the lines list
  string[] lines = new string[]{};

  //Chooses what lists to read into the lines list based on what choices the user has previously made
  if(weatherStation == "Lerwick"){

    switch(whatToView){
      case "meanMaxTemp":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS1_TMax.txt");
      break;

      case "meanMinTemp":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS1_TMin.txt");
      break;

      case "daysAirFrost":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS1_AF.txt");
      break;

      case "daysRainfall":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS1_Rain.txt");
      break;

      case "daysSun":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS1_Sun.txt");
      break;

      default:
        Console.WriteLine("Error");
      break;
    }
  } else{

    switch(whatToView){

      case "meanMaxTemp":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS2_TMax.txt");
      break;

      case "meanMinTemp":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS2_TMin.txt");
      break;

      case "daysAirFrost":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS2_AF.txt");
      break;

      case "daysRainfall":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS2_Rain.txt");
      break;

      case "daysSun":
          lines = System.IO.File.ReadAllLines(@"CMP1124M_Weather_Data/WS2_Sun.txt");
      break;

      default:
        Console.WriteLine("Error");
      break;
    }
  }

  //Specify the data array
  double[] data = new double[lines.Length];

  //Convert eacg element from a string into a double
  for(int x = 0; x < data.Length; x++){
    data[x] = Convert.ToDouble(lines[x]);
  }

  //Return the data array
  return data;
  }

}
