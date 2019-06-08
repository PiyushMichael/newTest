#include <iostream>         //notice no .h
#include <conio.h>
#include <stdlib.h>         //for system("CLS")
#include <windows.h>        //for console COORD and SetConsoleCursorPosition()
using namespace std;        //namespace for old functions and classes

#define clrscr() system("CLS")   //assigning system("CLS") to clrscr()
void gotoxy(int x,int y)         //custom defining gotoxy()
{COORD pos={x,y};
 SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE),pos);
}

int main()                       //main() must be of int type
{clrscr();
 gotoxy(10,10);
 cout<<"Hello world...";
 getch();
 clrscr();
}
