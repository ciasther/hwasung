<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기를 사용하여 수정하지 마십시오.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.label9 = New System.Windows.Forms.Label()
        Me.textBox2 = New System.Windows.Forms.TextBox()
        Me.comboBox5 = New System.Windows.Forms.ComboBox()
        Me.label7 = New System.Windows.Forms.Label()
        Me.textBox1 = New System.Windows.Forms.TextBox()
        Me.label6 = New System.Windows.Forms.Label()
        Me.comboBox4 = New System.Windows.Forms.ComboBox()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.serialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.label8 = New System.Windows.Forms.Label()
        Me.button9 = New System.Windows.Forms.Button()
        Me.button8 = New System.Windows.Forms.Button()
        Me.button7 = New System.Windows.Forms.Button()
        Me.button6 = New System.Windows.Forms.Button()
        Me.button5 = New System.Windows.Forms.Button()
        Me.button4 = New System.Windows.Forms.Button()
        Me.button3 = New System.Windows.Forms.Button()
        Me.button2 = New System.Windows.Forms.Button()
        Me.button1 = New System.Windows.Forms.Button()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.comboBox3 = New System.Windows.Forms.ComboBox()
        Me.comboBox2 = New System.Windows.Forms.ComboBox()
        Me.comboBox1 = New System.Windows.Forms.ComboBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'label9
        '
        Me.label9.AutoSize = True
        Me.label9.Location = New System.Drawing.Point(187, 276)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(85, 12)
        Me.label9.TabIndex = 49
        Me.label9.Text = "QR Code Data"
        '
        'textBox2
        '
        Me.textBox2.Location = New System.Drawing.Point(189, 291)
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New System.Drawing.Size(142, 21)
        Me.textBox2.TabIndex = 47
        '
        'comboBox5
        '
        Me.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBox5.FormattingEnabled = True
        Me.comboBox5.Location = New System.Drawing.Point(41, 292)
        Me.comboBox5.Name = "comboBox5"
        Me.comboBox5.Size = New System.Drawing.Size(142, 20)
        Me.comboBox5.TabIndex = 46
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(187, 230)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(81, 12)
        Me.label7.TabIndex = 45
        Me.label7.Text = "Barcode Data"
        '
        'textBox1
        '
        Me.textBox1.Location = New System.Drawing.Point(189, 248)
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New System.Drawing.Size(142, 21)
        Me.textBox1.TabIndex = 44
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(39, 230)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(85, 12)
        Me.label6.TabIndex = 43
        Me.label6.Text = "Barcode Type"
        '
        'comboBox4
        '
        Me.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBox4.FormattingEnabled = True
        Me.comboBox4.Location = New System.Drawing.Point(41, 248)
        Me.comboBox4.Name = "comboBox4"
        Me.comboBox4.Size = New System.Drawing.Size(142, 20)
        Me.comboBox4.TabIndex = 42
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(41, 66)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(38, 12)
        Me.label5.TabIndex = 41
        Me.label5.Text = "label5"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(39, 50)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(40, 12)
        Me.label4.TabIndex = 40
        Me.label4.Text = "Status"
        '
        'serialPort1
        '
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(39, 276)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(103, 12)
        Me.label8.TabIndex = 48
        Me.label8.Text = "QR Code Version"
        '
        'button9
        '
        Me.button9.Location = New System.Drawing.Point(43, 181)
        Me.button9.Name = "button9"
        Me.button9.Size = New System.Drawing.Size(140, 37)
        Me.button9.TabIndex = 39
        Me.button9.Text = "Blackmark Search && Cut"
        Me.button9.UseVisualStyleBackColor = True
        '
        'button8
        '
        Me.button8.Location = New System.Drawing.Point(337, 138)
        Me.button8.Name = "button8"
        Me.button8.Size = New System.Drawing.Size(142, 37)
        Me.button8.TabIndex = 38
        Me.button8.Text = "Partial Cut"
        Me.button8.UseVisualStyleBackColor = True
        '
        'button7
        '
        Me.button7.Location = New System.Drawing.Point(189, 138)
        Me.button7.Name = "button7"
        Me.button7.Size = New System.Drawing.Size(142, 37)
        Me.button7.TabIndex = 37
        Me.button7.Text = "Full Cut"
        Me.button7.UseVisualStyleBackColor = True
        '
        'button6
        '
        Me.button6.Location = New System.Drawing.Point(337, 282)
        Me.button6.Name = "button6"
        Me.button6.Size = New System.Drawing.Size(142, 37)
        Me.button6.TabIndex = 36
        Me.button6.Text = "QR Code Print"
        Me.button6.UseVisualStyleBackColor = True
        '
        'button5
        '
        Me.button5.Location = New System.Drawing.Point(337, 95)
        Me.button5.Name = "button5"
        Me.button5.Size = New System.Drawing.Size(142, 37)
        Me.button5.TabIndex = 35
        Me.button5.Text = "Pagemode(Standard)"
        Me.button5.UseVisualStyleBackColor = True
        '
        'button4
        '
        Me.button4.Location = New System.Drawing.Point(43, 138)
        Me.button4.Name = "button4"
        Me.button4.Size = New System.Drawing.Size(140, 37)
        Me.button4.TabIndex = 34
        Me.button4.Text = "Pagemode(Ticket)"
        Me.button4.UseVisualStyleBackColor = True
        '
        'button3
        '
        Me.button3.Location = New System.Drawing.Point(337, 239)
        Me.button3.Name = "button3"
        Me.button3.Size = New System.Drawing.Size(142, 37)
        Me.button3.TabIndex = 33
        Me.button3.Text = "Barcode Print"
        Me.button3.UseVisualStyleBackColor = True
        '
        'button2
        '
        Me.button2.Location = New System.Drawing.Point(189, 95)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(142, 37)
        Me.button2.TabIndex = 32
        Me.button2.Text = "Reciept Print"
        Me.button2.UseVisualStyleBackColor = True
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(43, 95)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(140, 37)
        Me.button1.TabIndex = 31
        Me.button1.Text = "Status Check"
        Me.button1.UseVisualStyleBackColor = True
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(343, 12)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(55, 12)
        Me.label3.TabIndex = 30
        Me.label3.Text = "Baudrate"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(198, 12)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(27, 12)
        Me.label2.TabIndex = 29
        Me.label2.Text = "Port"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(39, 12)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(40, 12)
        Me.label1.TabIndex = 28
        Me.label1.Text = "Model"
        '
        'comboBox3
        '
        Me.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBox3.FormattingEnabled = True
        Me.comboBox3.Location = New System.Drawing.Point(345, 27)
        Me.comboBox3.Name = "comboBox3"
        Me.comboBox3.Size = New System.Drawing.Size(139, 20)
        Me.comboBox3.TabIndex = 27
        '
        'comboBox2
        '
        Me.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBox2.FormattingEnabled = True
        Me.comboBox2.Location = New System.Drawing.Point(200, 27)
        Me.comboBox2.Name = "comboBox2"
        Me.comboBox2.Size = New System.Drawing.Size(139, 20)
        Me.comboBox2.TabIndex = 26
        '
        'comboBox1
        '
        Me.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBox1.FormattingEnabled = True
        Me.comboBox1.Location = New System.Drawing.Point(41, 27)
        Me.comboBox1.Name = "comboBox1"
        Me.comboBox1.Size = New System.Drawing.Size(153, 20)
        Me.comboBox1.TabIndex = 25
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(189, 181)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(142, 37)
        Me.Button10.TabIndex = 50
        Me.Button10.Text = "Firmware Ver"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(346, 195)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(0, 12)
        Me.Label10.TabIndex = 51
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(529, 344)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.label9)
        Me.Controls.Add(Me.textBox2)
        Me.Controls.Add(Me.comboBox5)
        Me.Controls.Add(Me.label7)
        Me.Controls.Add(Me.textBox1)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.comboBox4)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label8)
        Me.Controls.Add(Me.button9)
        Me.Controls.Add(Me.button8)
        Me.Controls.Add(Me.button7)
        Me.Controls.Add(Me.button6)
        Me.Controls.Add(Me.button5)
        Me.Controls.Add(Me.button4)
        Me.Controls.Add(Me.button3)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.comboBox3)
        Me.Controls.Add(Me.comboBox2)
        Me.Controls.Add(Me.comboBox1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VB"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label9 As System.Windows.Forms.Label
    Private WithEvents textBox2 As System.Windows.Forms.TextBox
    Private WithEvents comboBox5 As System.Windows.Forms.ComboBox
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents textBox1 As System.Windows.Forms.TextBox
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents comboBox4 As System.Windows.Forms.ComboBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents serialPort1 As System.IO.Ports.SerialPort
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents button9 As System.Windows.Forms.Button
    Private WithEvents button8 As System.Windows.Forms.Button
    Private WithEvents button7 As System.Windows.Forms.Button
    Private WithEvents button6 As System.Windows.Forms.Button
    Private WithEvents button5 As System.Windows.Forms.Button
    Private WithEvents button4 As System.Windows.Forms.Button
    Private WithEvents button3 As System.Windows.Forms.Button
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents button1 As System.Windows.Forms.Button
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents comboBox3 As System.Windows.Forms.ComboBox
    Private WithEvents comboBox2 As System.Windows.Forms.ComboBox
    Private WithEvents comboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label

End Class
