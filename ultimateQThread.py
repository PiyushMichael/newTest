import sys,time,random
from PyQt4.QtGui import *
from PyQt4.QtCore import *

class cutlet(QWidget):
    def __init__(self,parent=None):
        QWidget.__init__(self,parent)
        ##setting up the widget
        self.setWindowTitle('cutlet')
        self.label=QLabel('drawer sub-widget')
        self.board=QLabel()
        self.button=QPushButton('dont')
        self.board.setFixedSize(400,400)
        self.arrangement=QGridLayout()
        self.arrange=QGridLayout()
        ##a widget within a widget layout :O :O
        self.small=QWidget()
        self.arrangement.addWidget(self.label,0,0)
        self.arrangement.addWidget(self.button,0,1)
        self.small.setLayout(self.arrangement)
        self.arrange.addWidget(self.small,0,0)
        self.arrange.addWidget(self.board,1,0)
        self.setLayout(self.arrange)
        ##initialising pixmap
        self.pix=QPixmap(self.board.size())
        self.pix.fill(QColor(255,255,255))
        self.board.setPixmap(self.pix)
        ##setting up QThread
        self.thread=worker()
        ##connections
        self.connect(self.button,SIGNAL("clicked()"),self.starter_function)
        self.connect(self.thread,SIGNAL("UpdatePlease(int,int,int,int,int,int,int)"),self.update_this)
    def starter_function(self):
        ##initialising pixmap
        self.pix=QPixmap(self.board.size())
        self.pix.fill(QColor(255,255,255))
        self.board.setPixmap(self.pix)
        self.thread.starter(10000)                              ##set the number of iterations here
    def update_this(self,i,j,x,y,r,g,b):
        #print(i,j,x,y,r,g,b)
        painter=QPainter()
        painter.begin(self.pix)
        painter.setPen(QColor(r,g,b))
        painter.drawRect(i,j,x,y)
        painter.end()
        self.board.setPixmap(self.pix)

class worker(QThread):
    def __init__(self,parent=None):
        QThread.__init__(self,parent)
        self.k=0
    def starter(self,counts):
        print('starter invoked with the button')
        self.k=counts
        self.start()
    def run(self):
        for counter in range(self.k):
            random.seed()
            i=random.randrange(400)
            j=random.randrange(400)
            x=random.randrange(200)
            y=random.randrange(200)
            r=random.randrange(255)
            g=random.randrange(255)
            b=random.randrange(255)
            self.emit(SIGNAL("UpdatePlease(int,int,int,int,int,int,int)"),i,j,x,y,r,g,b)
            time.sleep(0.001)                               ##set wait time here for FPS

class big(QWidget):
    def __init__(self,parent=None):
        QWidget.__init__(self,parent)
        panel1=cutlet()
        panel2=cutlet()
        panel3=cutlet()
        thelayout=QGridLayout()
        thelayout.addWidget(panel1,0,0)
        thelayout.addWidget(panel2,0,1)
        thelayout.addWidget(panel3,0,2)
        self.setLayout(thelayout)

if __name__=='__main__':
    app=QApplication(sys.argv)
    one=big()
    one.show()
    app.exec_()