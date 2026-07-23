Imports System.IO.Ports
Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        comboBox1.Items.Add("HMK-060")
        comboBox1.Items.Add("HMK-080")
        comboBox1.Items.Add("HMK-081")
        comboBox1.Items.Add("HMK-825")
        comboBox1.Items.Add("HMK-830")
        comboBox1.Items.Add("HMK-072")
        comboBox1.Items.Add("HMK-054")
        comboBox1.Items.Add("HP-083")
        comboBox1.Items.Add("HP-283")
        comboBox1.SelectedIndex = 0


        comboBox2.Items.Add("USB")
        comboBox2.Items.Add("COM1")
        comboBox2.Items.Add("COM2")
        comboBox2.Items.Add("COM3")
        comboBox2.Items.Add("COM4")
        comboBox2.Items.Add("COM5")
        comboBox2.Items.Add("COM6")
        comboBox2.Items.Add("COM7")
        comboBox2.SelectedIndex = 0

        comboBox3.Items.Add("9600")
        comboBox3.Items.Add("19200")
        comboBox3.Items.Add("38400")
        comboBox3.Items.Add("57600")
        comboBox3.Items.Add("115200")
        comboBox3.SelectedIndex = 0

        comboBox4.Items.Add("UPC-E")
        comboBox4.Items.Add("EAN13")
        comboBox4.Items.Add("EAN8")
        comboBox4.Items.Add("CODE39")
        comboBox4.Items.Add("ITF(I of 2/5)")
        comboBox4.Items.Add("CODABAR")
        comboBox4.Items.Add("CODE128 A")
        comboBox4.Items.Add("CODE128 B")
        comboBox4.Items.Add("CODE128 C")
        comboBox4.SelectedIndex = 0

        comboBox5.Items.Add("Version 1")
        comboBox5.Items.Add("Version 3")
        comboBox5.Items.Add("Version 5")
        comboBox5.Items.Add("Version 9")
        comboBox5.SelectedIndex = 0

        label5.Text = ""
        textBox1.Text = "1234567890"
        textBox2.Text = "1234567890"
    End Sub
    Public Function port_open(ByVal port As Object, ByVal baudrate As Integer)

        port_open = 20
        Dim a As Integer
        If port.Equals("USB") Then

            a = UsbOpen(comboBox1.SelectedItem.ToString())
            port_open = a

        Else

            If serialPort1.IsOpen = True Then

                serialPort1.Close()        'Serialport close
            End If

            serialPort1.PortName = port          'serial port
            serialPort1.DataBits = 8                                  'set databit
            serialPort1.StopBits = StopBits.One                            'set stopbit
            serialPort1.Parity = Parity.None                               'set parity
            serialPort1.Handshake = Handshake.RequestToSend                'set handshake RTS
            serialPort1.RtsEnable = True
            serialPort1.Encoding = System.Text.Encoding.Default            'set encoding
            'serialPort1.DataReceived = New SerialDataReceivedEventHandler(DataReceivedHandler)

            Select Case (baudrate)                                'select baudrate

                Case 0
                    serialPort1.BaudRate = 9600
                Case 1
                    serialPort1.BaudRate = 19200
                Case 2
                    serialPort1.BaudRate = 38400
                Case 3
                    serialPort1.BaudRate = 57600
                Case 4
                    serialPort1.BaudRate = 115200
            End Select
            If (serialPort1.IsOpen = False) Then

                serialPort1.Open()     'Serial Port open
                port_open = 10
            End If
            End If

        'Return
    End Function
    Private Sub DataReceivedHandler(ByVal sender As Object, e As SerialDataReceivedEventArgs) Handles serialPort1.DataReceived


        Dim sp As SerialPort = sender
        Dim str As String = sp.ReadExisting
        Dim data() As Byte
        ReDim data(str.Length)
        data = System.Text.ASCIIEncoding.ASCII.GetBytes(str)
        If str.Length <> 0 Then
            If str.Length <= 2 Then
                prt_status(data(0))
            Else
                prt_ver(data)

            End If
        End If
    End Sub
    Delegate Sub SetTextCallback(ByVal text As String, ByVal obj As String)
    Private Sub SetText(text As String, obj As String)

        If Me.InvokeRequired Then

            Dim d As SetTextCallback = New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {text, obj})

        Else
            If obj = 5 Then
                Me.label5.Text = text
            ElseIf obj = 10 Then
                Me.Label10.Text = text

            End If
        End If
    End Sub
    Private Sub prt_ver(ver As Byte())
        Dim test As String
        For i = 0 To ver.Length - 1
            test = test + Chr(ver(i))
        Next
        SetText(test, "10")
    End Sub
    Private Sub prt_status(status As Byte)

        Select Case (status)
            Case 0
                SetText("Normal Status", "5")
            Case 1
                SetText("Paper out", "5")
            Case 2
                SetText("Head open", "5")
            Case 3
                SetText("Paper out && Head open", "5")
            Case 4
                SetText("Paper Jam", "5")
            Case 5
                SetText("Paper out && Paper Jam", "5")
            Case 6
                SetText("Head open && Paper Jam", "5")
            Case 7
                SetText("Paper out && Head open && Paper Jam", "5")
            Case 8
                SetText("Near End", "5")
            Case 9
                SetText("Paper out && Near end", "5")
            Case 10
                SetText("Head open && Near end", "5")
            Case 11
                SetText("Paper out && Head open && Near end", "5")
            Case 12
                SetText("Paper Jam && Near end", "5")
            Case 13
                SetText("Paper out && Paper Jam && Near end", "5")
            Case 14
                SetText("Head open && Paper Jam && Near end", "5")
            Case 15
                SetText("Paper out && Head open && Paper Jam && Near end", "5")
            Case 16
                SetText("Print Running", "5")
            Case 32
                SetText("Cutter Jam", "5")
        End Select
    End Sub

    Private Sub button1_Click(sender As Object, e As EventArgs)
        'printer status
        If port_open(comboBox2.SelectedItem.ToString, comboBox3.SelectedIndex) = 10 Then  'serial
            Dim send() As Byte = New Byte() {&H10, &H4, &H2}
            serialPort1.Write(send, 0, send.Length)
        ElseIf port_open(comboBox2.SelectedItem.ToString, comboBox3.SelectedIndex) = 0 Then 'usb
            prt_status(NewRealRead())
        End If
    End Sub
    Private Sub button2_Click(sender As Object, e As EventArgs) Handles button2.Click
        If port_open(comboBox2.SelectedItem.ToString, comboBox3.SelectedIndex) = 10 Then

            Dim MyData() As Byte
            ReDim MyData(1)
            serialPort1.Write(Chr(&H1A) & "x" & Chr(&H1))                       'Extended ascii mode 
            serialPort1.Write(Chr(&H1B) & "a" & Chr(&H1))                       'Text Align center
            serialPort1.Write("Starbucks Coffee Germany" & Chr(&HA) & Chr(&HA))
            serialPort1.Write("Frankfrut am Main" & Chr(&HA))
            serialPort1.Write("Kaiserstra")
            MyData(0) = 225
            serialPort1.Write(MyData, 0, 1)
            serialPort1.Write("e" & Chr(&HA))
            serialPort1.Write("Tele:" & Chr(&HA))
            serialPort1.Write("VAT:  6417373R" & Chr(&HA) & Chr(&HA))
            serialPort1.Write(Chr(&H1D) & "L" & Chr(&H12) & Chr(&H0))
            serialPort1.Write(" 100003 Claire 0" & Chr(&HA))
            serialPort1.Write("----------------------------------------" & Chr(&HA))
            serialPort1.Write("Chk 806             13Oct'16 15:48" & Chr(&HA))
            serialPort1.Write("----------------------------------------" & Chr(&HA))
            serialPort1.Write(Chr(&H1D) & "L" & Chr(&H32) & Chr(&H0))
            serialPort1.Write(Chr(&H1D) & "!" & Chr(&H10))
            serialPort1.Write("To Go" & Chr(&HA))
            serialPort1.Write(Chr(&H1D) & "!" & Chr(&H0))
            serialPort1.Write("Gr Carml Macchiato                4.65" & Chr(&HA))
            serialPort1.Write("  Decaf" & Chr(&HA))
            serialPort1.Write("Gr Latte                          3.95" & Chr(&HA))
            serialPort1.Write("  Hazelnut                        0.05" & Chr(&HA))
            serialPort1.Write("Tl Chai Tea Latte                 3.45" & Chr(&HA))
            serialPort1.Write("Visa                             13.77" & Chr(&HA))
            serialPort1.Write("XXXXXXXXXXXX4258" & Chr(&HA) & Chr(&HA))
            serialPort1.Write("Subtotal                    EURO 12.55" & Chr(&HA))
            serialPort1.Write("Tax 9.75%                   EURO  1.22" & Chr(&HA))
            serialPort1.Write("Total                       EURO 13.77" & Chr(&HA))
            serialPort1.Write("Change Due                  EURO  0.00" & Chr(&HA) & Chr(&HA))
            serialPort1.Write("========================================" & Chr(&HA))
            serialPort1.Write("Thank you. Please visit us again" & Chr(&HA))
            serialPort1.Write("For more information visit" & Chr(&HA))
            serialPort1.Write("www.starbucks.de" & Chr(&HA) & Chr(&HA) & Chr(&HA) & Chr(&HA) & Chr(&HA) & Chr(&HA))

            serialPort1.Write(Chr(&H1B) & "i")                                       'Full Cut
            serialPort1.Write(Chr(&H1B) & "a" & Chr(&H0))                           'Text Align left

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            a = PrintCmd(&H1A)
            a = PrintStr("x")
            a = PrintCmd(&H1)                       'Extended ascii mode 
            a = PrintCmd(&H1B)
            a = PrintStr("a")
            a = PrintCmd(&H1)                       'Text Align center
            a = PrintStr("Starbucks Coffee Germany")
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintStr("Frankfrut am Main")
            a = PrintCmd(&HA)
            a = PrintStr("Kaiserstra")
            a = PrintCmd(225)
            a = PrintStr("e")
            a = PrintCmd(&HA)
            a = PrintStr("Tele:")
            a = PrintCmd(&HA)
            a = PrintStr("VAT:  6417373R")
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintCmd(&H1D)
            a = PrintStr("L")
            a = PrintCmd(&H12)
            a = PrintCmd(&H0)
            a = PrintStr(" 100003 Claire 0")
            a = PrintCmd(&HA)
            a = PrintStr("----------------------------------------")
            a = PrintCmd(&HA)
            a = PrintStr("Chk 806             13Oct'16 15:48")
            a = PrintCmd(&HA)
            a = PrintStr("----------------------------------------")
            a = PrintCmd(&HA)
            a = PrintCmd(&H1D)
            a = PrintStr("L")
            a = PrintCmd(&H32)
            a = PrintCmd(&H0)
            a = PrintCmd(&H1D)
            a = PrintStr("!")
            a = PrintCmd(&H10)
            a = PrintStr("To Go")
            a = PrintCmd(&H1D)
            a = PrintStr("!")
            a = PrintCmd(&H0)
            a = PrintCmd(&HA)
            a = PrintStr("Gr Carml Macchiato                4.65")
            a = PrintCmd(&HA)
            a = PrintStr("  Decaf")
            a = PrintCmd(&HA)
            a = PrintStr("Gr Latte                          3.95")
            a = PrintCmd(&HA)
            a = PrintStr("  Hazelnut                        0.05")
            a = PrintCmd(&HA)
            a = PrintStr("Tl Chai Tea Latte                 3.45")
            a = PrintCmd(&HA)
            a = PrintStr("Visa                             13.77")
            a = PrintCmd(&HA)
            a = PrintStr("XXXXXXXXXXXX4258")
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintStr("Subtotal                    EURO 12.55")
            a = PrintCmd(&HA)
            a = PrintStr("Tax 9.75%                   EURO  1.22")
            a = PrintCmd(&HA)
            a = PrintStr("Total                       EURO 13.77")
            a = PrintCmd(&HA)
            a = PrintStr("Change Due                  EURO  0.00")
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintStr("========================================")
            a = PrintCmd(&HA)
            a = PrintStr("Thank you. Please visit us again")
            a = PrintCmd(&HA)
            a = PrintStr("For more information visit")
            a = PrintCmd(&HA)
            a = PrintStr("www.starbucks.de")
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)
            a = PrintCmd(&HA)

            a = PrintCmd(&H1B)
            a = PrintStr("i")                                       'Full Cut
            a = PrintCmd(&H1B)
            a = PrintStr("a")
            a = PrintCmd(&H0)                           'Text Align left
        End If
    End Sub

    Private Sub button5_Click(sender As Object, e As EventArgs) Handles button5.Click
        If port_open(comboBox2.SelectedItem.ToString, comboBox3.SelectedIndex) = 10 Then

            Dim MyData(8) As Byte

            serialPort1.Write(Chr(&H1B) & "L")           ' PAGE MODE
            serialPort1.Write(Chr(&H1B) & "T" & Chr(&H0))  ' PAGE TOWARD

            serialPort1.Write(Chr(&H1B) & "W")

            MyData(0) = &H0 'xL
            MyData(1) = &H0 'xH
            MyData(2) = &H0 'yL
            MyData(3) = &H0 'yH
            MyData(4) = &HA0 'dxL
            MyData(5) = &H0 'dxH
            MyData(6) = &HD9 'dyL
            MyData(7) = &H0 'dyH

            'location x = 0mm      
            'location y = 0mm
            'dx =  20mm
            'dy =  27.125mm         
            serialPort1.Write(MyData, 0, 8)
            serialPort1.Write("ABCDEFGHIJKLM")


            serialPort1.Write(Chr(&H1B) & "W")
            MyData(0) = &H70 'xL
            MyData(1) = &H0 'xH
            MyData(2) = &H90 'yL
            MyData(3) = &H0 'yH
            MyData(4) = &HA0 'dxL
            MyData(5) = &H0 'dxH
            MyData(6) = &HD9 'dyL
            MyData(7) = &H0 'dyH


            'location x = 14mm
            'location y = 18mm
            'dx =  20mm
            'dy =  27.125mm         
            serialPort1.Write(MyData, 0, 8)
            serialPort1.Write("1234567890123")


            serialPort1.Write(Chr(&H1B) & "W")
            MyData(0) = &HA0 'xL
            MyData(1) = &H0 'xH
            MyData(2) = &H60 'yL
            MyData(3) = &H0 'yH
            MyData(4) = &HA0 'dxL
            MyData(5) = &H0 'dxH
            MyData(6) = &HD9 'dyL
            MyData(7) = &H0 'dyH

            'location x = 20mm
            'location y = 12mm
            'dx =  20mm
            'dy =  27.125mm      
            serialPort1.Write(MyData, 0, 8)
            serialPort1.Write("123ABC456DEF7")

            serialPort1.Write(Chr(&H1B) & Chr(&HC))   ' PAGE AREA PRINT

            serialPort1.Write((&H1B) & "S")       ' PAGE AREA CLEAR AND TO STANDARD MODE


        ElseIf port_open(comboBox2.SelectedItem.ToString, comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            Dim MyData(8) As Byte
            a = PrintCmd(&H1B)
            a = PrintStr("L")           ' PAGE MODE

            a = PrintCmd(&H1B)
            a = PrintStr("T")
            a = PrintCmd(&H0)   ' PAGE TOWARD

            a = PrintCmd(&H1B)
            a = PrintStr("W")

            a = PrintCmd(&H0) 'xL
            a = PrintCmd(&H0) 'xH
            a = PrintCmd(&H0) 'yL
            a = PrintCmd(&H0) 'yH
            a = PrintCmd(&HA0) 'dxL
            a = PrintCmd(&H0) 'dxH
            a = PrintCmd(&HD9) 'dyL
            a = PrintCmd(&H0) 'dyH

            'location x = 0mm      
            'location y = 0mm
            'dx =  20mm
            'dy =  27.125mm         
            a = PrintStr("ABCDEFGHIJKLM")


            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintCmd(&H70) 'xL
            a = PrintCmd(&H0) 'xH
            a = PrintCmd(&H90) 'yL
            a = PrintCmd(&H0) 'yH
            a = PrintCmd(&HA0) 'dxL
            a = PrintCmd(&H0) 'dxH
            a = PrintCmd(&HD9) 'dyL
            a = PrintCmd(&H0) 'dyH

            'location x = 14mm
            'location y = 18mm
            'dx =  20mm
            'dy =  27.125mm         
            a = PrintStr("1234567890123")


            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintCmd(&HA0) 'xL
            a = PrintCmd(&H0) 'xH
            a = PrintCmd(&H60) 'yL
            a = PrintCmd(&H0) 'yH
            a = PrintCmd(&HA0) 'dxL
            a = PrintCmd(&H0) 'dxH
            a = PrintCmd(&HD9) 'dyL
            a = PrintCmd(&H0) 'dyH

            'location x = 20mm
            'location y = 12mm
            'dx =  20mm
            'dy =  27.125mm      
            a = PrintStr("123ABC456DEF7")

            a = PrintCmd(&H1B)
            a = PrintCmd(&HC)         ' PAGE AREA PRINT

            a = PrintCmd(&H1B)
            a = PrintStr("S")          ' PAGE AREA CLEAR AND TO STANDARD MODE
        End If
    End Sub

    Private Sub button4_Click(sender As Object, e As EventArgs) Handles button4.Click
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then

            serialPort1.Write(Chr(&H1B) & "L")           ' PAGE MODE
            serialPort1.Write(Chr(&H1B) & "T" & Chr(&H1))  ' PAGE 방향
            ' 좌표지정 및 문자열출력

            serialPort1.Write(Chr(&H1B) & "W" & "0010" & "1160")
            serialPort1.Write("Thermal Printer Ticket Sample")

            serialPort1.Write(Chr(&H1B) & "W" & "0104" & "1160")
            serialPort1.Write("Hwasung System Thermal Printer")

            serialPort1.Write(Chr(&H1B) & "W" & "0136" & "1160")
            serialPort1.Write("Sample Print")

            serialPort1.Write(Chr(&H1D) & "!" & Chr(&H0)) ' DOUBLE WIDTH SIZE
            serialPort1.Write(Chr(&H1B) & "W" & "0216" & "1160")
            serialPort1.Write(DateTime.Now.ToString())

            serialPort1.Write(Chr(&H1B) & "W" & "0280" & "1160")
            serialPort1.Write("Page Mode Ticket Sample")

            serialPort1.Write(Chr(&H1D) & "!" & Chr(&H10)) ' DOUBLE HEIGHT SIZE
            serialPort1.Write(Chr(&H1B) & "W" & "0104" & "0304")
            serialPort1.Write("Page Mode")

            serialPort1.Write(Chr(&H1D) & "!" & Chr(&H0))  'NORMAL SIZE
            serialPort1.Write(Chr(&H1B) & "W" & "0168" & "0304")
            serialPort1.Write("Sample Print")

            serialPort1.Write(Chr(&H1B) & "W" & "0416" & "1160")
            serialPort1.Write("Thermal Printer Ticket Sample")


            '--------- BARCODE --------------------------------
            serialPort1.Write(Chr(&H1B) & "W" & "0376" & "0688")
            serialPort1.Write(Chr(&H1D) & "h" & Chr(40))  ' barcode height
            serialPort1.Write(Chr(&H1D) & "k" & Chr(5)) ' barcode type
            serialPort1.Write("010001200307311439" & Chr(0))  ' barcode data


            serialPort1.Write("" & Chr(&H1B) & Chr(&HC))  ' PAGE AREA PRINT

            serialPort1.Write(Chr(&H1B) & "S")       ' PAGE AREA CLEAR AND TO STANDARD MODE


            ' -----------------------

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            a = PrintCmd(&H1B)
            a = PrintStr("L")           ' PAGE MODE SET
            a = PrintCmd(&H1B)
            a = PrintStr("T")
            a = PrintCmd(&H1)   ' PAGE TOWARD

            ' 좌표지정 및 문자열출력
            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0010")
            a = PrintStr("1160")
            a = PrintStr("Thermal Printer Ticket Sample")

            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0104")
            a = PrintStr("1160")
            a = PrintStr("Hwasung System Thermal Printer")

            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0136")
            a = PrintStr("1160")
            a = PrintStr("Sample Print")

            a = PrintCmd(&H1D)
            a = PrintStr("!")
            a = PrintCmd(&H0)

            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0216")
            a = PrintStr("1160")
            a = PrintStr(DateTime.Now.ToString())

            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0280")
            a = PrintStr("1160")
            a = PrintStr("Page Mode Ticket Sample")

            a = PrintCmd(&H1D)
            a = PrintStr("!")
            a = PrintCmd(&H10)

            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0104")
            a = PrintStr("0304")
            a = PrintStr("Page Mode")

            a = PrintCmd(&H1D)
            a = PrintStr("!")
            a = PrintCmd(&H0)

            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0168")
            a = PrintStr("0304")
            a = PrintStr("Sample Print")

            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0416")
            a = PrintStr("1160")
            a = PrintStr("Thermal Printer Ticket Sample")


            '--------- BARCODE --------------------------------
            a = PrintCmd(&H1B)
            a = PrintStr("W")
            a = PrintStr("0376")
            a = PrintStr("0688")
            a = PrintCmd(&H1D) ' barcode height
            a = PrintStr("h")
            a = PrintCmd(&H40)
            a = PrintCmd(&H1D) ' barcode type
            a = PrintStr("k")
            a = PrintCmd(&H5)
            a = PrintStr("010001200307311439")
            a = PrintCmd(&H0)   ' barcode data

            a = PrintCmd(&H1B) ' PAGE AREA PRINT
            a = PrintCmd(&HC)

            ' PAGE AREA CLEAR AND TO STANDARD MODE
            a = PrintCmd(&H1B)
            a = PrintStr("S")
        End If
    End Sub

    Private Sub button7_Click(sender As Object, e As EventArgs) Handles button7.Click
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then

            serialPort1.Write(Chr(&H1B) + "i")

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            a = PrintCmd(&H1B)
            a = PrintStr("i")
        End If
    End Sub

    Private Sub button8_Click(sender As Object, e As EventArgs) Handles button8.Click
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then

            serialPort1.Write(Chr(&H1B) + "m")

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            a = PrintCmd(&H1B)
            a = PrintStr("m")
        End If
    End Sub

    Private Sub button9_Click(sender As Object, e As EventArgs) Handles button9.Click
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then

            serialPort1.Write(Chr(&H13) + "i")

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            a = PrintCmd(&H13)
            a = PrintStr("i")
        End If
    End Sub

    Private Sub button3_Click(sender As Object, e As EventArgs) Handles button3.Click
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then
            If comboBox4.SelectedIndex + 1 >= 7 Then
                serialPort1.Write(Chr(&H1D) & "k" & Chr(7))
                Select Case (comboBox4.SelectedIndex + 1)
                    Case 7
                        serialPort1.Write("g" & textBox1.Text & Chr(&H0))
                    Case 8
                        serialPort1.Write("h" & textBox1.Text & Chr(&H0))
                    Case 9
                        serialPort1.Write("i" & textBox1.Text & Chr(&H0))
                End Select
            Else
                If comboBox4.SelectedIndex + 1 = 1 Or comboBox4.SelectedIndex + 1 = 3 Then
                    If textBox1.TextLength < 7 Then
                        textBox1.Text = "0123456"
                    End If
                    serialPort1.Write(Chr(&H1D) & "k" & Chr(comboBox4.SelectedIndex + 1) & textBox1.Text.Substring(0, 7) & Chr(&H0))
                ElseIf comboBox4.SelectedIndex + 1 = 2 Then
                    If textBox1.TextLength < 12 Then
                        textBox1.Text = "012345678901"
                    End If
                    serialPort1.Write(Chr(&H1D) & "k" & Chr(comboBox4.SelectedIndex + 1) & textBox1.Text.Substring(0, 12) & Chr(&H0))
                    ElseIf comboBox4.SelectedIndex + 1 = 5 Then
                        If textBox1.TextLength Mod 2 = 0 Then
                            serialPort1.Write(Chr(&H1D) & "k" & Chr(comboBox4.SelectedIndex + 1) & textBox1.Text & Chr(&H0))
                        Else
                            serialPort1.Write(Chr(&H1D) & "k" & Chr(comboBox4.SelectedIndex + 1) & textBox1.Text + "0" & Chr(&H0))
                    End If
                Else
                    serialPort1.Write(Chr(&H1D) & "k" & Chr(comboBox4.SelectedIndex + 1) & textBox1.Text & Chr(&H0))
                End If
        End If
        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then
            Dim a As Integer
            If comboBox4.SelectedIndex + 1 >= 7 Then
                a = PrintCmd(&H1D)
                a = PrintStr("k")
                a = PrintCmd(&H7)
                Select Case (comboBox4.SelectedIndex + 1)
                    Case 7
                        a = PrintStr("g")
                        a = PrintStr(textBox1.Text)
                        a = PrintCmd(&H0)
                    Case 8
                        a = PrintStr("h")
                        a = PrintStr(textBox1.Text)
                        a = PrintCmd(&H0)
                    Case 9
                        a = PrintStr("i")
                        a = PrintStr(textBox1.Text)
                        a = PrintCmd(&H0)
                End Select
            Else
                If comboBox4.SelectedIndex + 1 = 1 Or comboBox4.SelectedIndex + 1 = 3 Then
                    If textBox1.TextLength < 7 Then
                        textBox1.Text = "0123456"
                    End If
                    a = PrintCmd(&H1D)
                    a = PrintStr("k")
                    a = PrintCmd((comboBox4.SelectedIndex + 1))
                    a = PrintStr(textBox1.Text.Substring(0, 7))
                    a = PrintCmd(&H0)

                ElseIf comboBox4.SelectedIndex + 1 = 2 Then
                    If textBox1.TextLength < 12 Then
                        textBox1.Text = "012345678901"
                    End If
                    a = PrintCmd(&H1D)
                    a = PrintStr("k")
                    a = PrintCmd((comboBox4.SelectedIndex + 1))
                    a = PrintStr(textBox1.Text.Substring(0, 12))
                    a = PrintCmd(&H0)

                ElseIf comboBox4.SelectedIndex + 1 = 5 Then
                    If textBox1.TextLength Mod 2 = 0 Then
                        a = PrintCmd(&H1D)
                        a = PrintStr("k")
                        a = PrintCmd(comboBox4.SelectedIndex + 1)
                        a = PrintStr(textBox1.Text)
                        a = PrintCmd(&H0)
                    Else
                        a = PrintCmd(&H1D)
                        a = PrintStr("k")
                        a = PrintCmd(comboBox4.SelectedIndex + 1)
                        a = PrintStr(textBox1.Text + "0")
                        a = PrintCmd(&H0)
                    End If
                Else
                    a = PrintCmd(&H1D)
                    a = PrintStr("k")
                    a = PrintCmd(comboBox4.SelectedIndex + 1)
                    a = PrintStr(textBox1.Text)
                    a = PrintCmd(&H0)
                End If
            End If
        End If
    End Sub

    Private Sub button6_Click(sender As Object, e As EventArgs) Handles button6.Click
        Dim str As String = comboBox5.SelectedItem.ToString()
        Dim val As String = Str.Remove(0, Str.Length - 1)
        Dim cnt As Integer
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then

            cnt = textBox2.TextLength
            serialPort1.Write(Chr(&H1A) & "B" & Chr(&H2))
            serialPort1.Write("" & Chr(cnt))
            Select Case (val)
                Case "1"
                    serialPort1.Write("" & Chr(&H1))

                Case "3"
                    serialPort1.Write("" & Chr(&H3))

                Case "5"
                    serialPort1.Write("" & Chr(&H5))

                Case "9"
                    serialPort1.Write("" & Chr(&H9))

            End Select
            serialPort1.Write(textBox2.Text)

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            cnt = textBox2.TextLength
            a = PrintCmd(&H1A)
            a = PrintStr("B")
            a = PrintCmd(&H2)
            a = PrintCmd(cnt)
            Select Case (val)

                Case "1"
                    a = PrintCmd(&H1)

                Case "3"
                    a = PrintCmd(&H3)

                Case "5"
                    a = PrintCmd(&H5)

                Case "9"
                    a = PrintCmd(&H9)

            End Select
            a = PrintStr(textBox2.Text)
        End If

    End Sub

    Private Sub button1_Click_1(sender As Object, e As EventArgs) Handles button1.Click
        'printer status
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then  'serial

            Dim send() As Byte = New Byte() {&H10, &H4, &H2}
            serialPort1.Write(send, 0, send.Length)

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then 'usb

            prt_status(NewRealRead())
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim ver As String
        If port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 10 Then

            serialPort1.Write(Chr(&H1D) + "I" + "A")

        ElseIf port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) = 0 Then

            Dim a As Integer
            a = PrintCmd(&H1D)
            a = PrintStr("I")
            a = PrintStr("A")

            ver = Chr(DummyRealRead)
            ver = ver + Chr(DummyRealRead)
            ver = ver + Chr(DummyRealRead)
            ver = ver + Chr(DummyRealRead)
            Label10.Text = ver
        End If
    End Sub
End Class
