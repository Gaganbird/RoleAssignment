cd ..\..\..\..\..\..\DesktopModules\Vanjaro\UXManager\Extensions\Menu\RoleAssignment\
rd bin /s /q
md bin
copy ..\..\..\..\..\..\bin\JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.dll bin\
del ..\..\..\..\..\..\Install\Module\RoleAssignmnent_1.0.0_DNN73_Install.zip
del Resources.zip
"C:\Program Files\7-Zip\7z.exe" a Resources.zip @Resources.txt -xr!?svn\
"C:\Program Files\7-Zip\7z.exe" a ..\..\..\..\..\..\Install\Module\RoleAssignmnent_1.0.0_DNN73_Install.zip @PackageList.txt
pause
