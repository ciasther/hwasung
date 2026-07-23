Module Module1
    Declare Function UsbOpen Lib "HwaUSB" (ByVal a As String) As Integer
    Declare Function UsbClose Lib "HwaUSB" ()
    Declare Function PrintStr Lib "HwaUSB" (ByVal a As String) As Integer
    Declare Function PrintCmd Lib "HwaUSB" (ByVal a As Integer) As Integer
    Declare Function RealRead Lib "HwaUSB" () As Integer
    Declare Function NewRealRead Lib "HwaUSB" () As Integer
    Declare Function DummyRealRead Lib "HwaUSB" () As Integer
    Declare Function PrintPacket Lib "HwaUSB" (ByRef PacketBuf As Byte, ByVal PacketLen As Long) As Integer
End Module
