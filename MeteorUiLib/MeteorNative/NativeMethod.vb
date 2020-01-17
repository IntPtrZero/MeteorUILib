Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel

Namespace Native
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Module NativeMethod
        Public Const WM_SYSCOMMAND As Integer = &H112
        Public Const SC_CLOSE As Integer = &HF060
        Public Const SC_MAXIMIZE As Integer = &HF030
        Public Const SC_MINIMIZE As Integer = &HF020
        Public Const SC_RESTORE As Integer = &HF120

        Public Const WM_NCHITTEST As Integer = &H84
        Public Const WM_NCLBUTTONDOWN As Integer = &HA1
        Public Const WM_NCLBUTTONUP As Integer = &HA2
        Public Const WM_NCMOUSELEAVE As Integer = &H2A2
        Public Const WM_WINDOWPOSCHANGING As Integer = &H46
        Public Const WM_WINDOWPOSCHANGED As Integer = &H47
        Public Const WM_DPICHANGED As Integer = &H2E0
        Public Const WM_NCCALCSIZE As Integer = &H83
        Public Const WM_GETMINMAXINFO As Integer = &H24

        Public Const HTBOTTOM As Integer = 15
        Public Const HTBOTTOMLEFT As Integer = 16
        Public Const HTBOTTOMRIGHT As Integer = 17
        Public Const HTCAPTION As Integer = 2
        Public Const HTLEFT As Integer = 10
        Public Const HTRIGHT As Integer = 11
        Public Const HTTOP As Integer = 12
        Public Const HTTOPLEFT As Integer = 13
        Public Const HTTOPRIGHT As Integer = 14

        Public Const WS_MINIMIZEBOX As Integer = &H20000
        Public Const WS_MAXIMIZEBOX As Integer = &H10000
        Public Const WS_THICKFRAME As Integer = &H40000
        Public Const WS_CAPTION As Integer = &HC00000

        Public Const AW_HOR_POSITIVE As Integer = &H1
        Public Const AW_HOR_NEGATIVE As Integer = &H2
        Public Const AW_VER_POSITIVE As Integer = &H4
        Public Const AW_VER_NEGATIVE As Integer = &H8
        Public Const AW_CENTER As Integer = &H10
        Public Const AW_HIDE As Integer = &H10000
        Public Const AW_ACTIVATE As Integer = &H20000
        Public Const AW_SLIDE As Integer = &H40000
        Public Const AW_BLEND As Integer = &H80000

        Public Const WHEEL_DELTA As Integer = 120

        Public Const _
            SW_HIDE = 0,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_MAX = 10

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure NCCALCSIZE_PARAMS
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> _
            Public recs As RECT()
            Public lppos As WINDOWPOS
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WINDOWPOS
            Public hwndInsertAfter As IntPtr
            Public hwnd As IntPtr
            Public x As Integer
            Public y As Integer
            Public cx As Integer
            Public cy As Integer
            Public uflag As UInteger
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure RECT
            Public Left As Integer
            Public Top As Integer
            Public Right As Integer
            Public Bottom As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure MINMAXINFO
            Public ptReserved As Point
            Public ptMaxSize As Size
            Public ptMaxPosition As Point
            Public ptMinTrackSize As Size
            Public ptMaxTrackSize As Size
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WINDOWPLACEMENT
            Public length As UInteger
            Public flags As UInteger
            Public showCmd As UInteger
            Public ptMinPosition As Point
            Public ptMaxPosition As Point
            Public rcNormalPosition As RECT
            Public rcDevice As RECT
        End Structure

        <DllImport("user32.dll")> _
        Public Function AnimateWindow(ByVal hWnd As IntPtr, ByVal dwTime As UInteger, ByVal dwFlags As UInteger) As Boolean
        End Function

        <DllImport("user32.dll")> _
        Public Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
        End Function

        <DllImport("user32.dll")> _
        Public Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlag As UInteger) As Boolean
        End Function

        <DllImport("user32.dll")> _
        Public Function GetWindowPlacement(ByVal hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean
        End Function

        <DllImport("user32.dll")>
        Public Function SetProcessDPIAware() As Boolean
        End Function

        <DllImport("user32.dll")>
        Public Function ScrollWindowEx(ByVal hWnd As IntPtr, ByVal dx As Integer, ByVal dy As Integer, ByVal prcScroll As IntPtr, ByVal prcClip As IntPtr, ByVal hrgnUpdate As IntPtr, ByVal prcUpdate As IntPtr, ByVal uFlags As UInteger) As Integer
        End Function

        <DllImport("user32.dll")>
        Public Function ReleaseDC(ByVal hWnd As IntPtr, ByVal hdc As IntPtr) As Boolean
        End Function
    End Module
End Namespace
