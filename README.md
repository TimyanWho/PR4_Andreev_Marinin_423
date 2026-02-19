Поддержка и тестирование программных модулей
ПРАКТИЧЕСКАЯ РАБОТА #4 ТЕСТИРОВАНИЕ "БЕЛЫМ ЯЩИКОМ"

Цель работы: приобрести практические навыки ручного тестирования
методом "белого ящика".

Маринин Степан и Андреев Тимофей, 3ИСИП-423

- варианты задания со скриншотами функций:
<img width="351" height="87" alt="f1" src="https://github.com/user-attachments/assets/0d6e0d52-573f-4603-823e-d62bc73b25a6" />
<img width="414" height="110" alt="f2" src="https://github.com/user-attachments/assets/069ff42f-8e63-44c2-bb88-51302e6d9096" />
<img width="375" height="119" alt="f3" src="https://github.com/user-attachments/assets/ea6a96a2-d464-4b38-b3e7-1bbbddb55da3" />

- стек технологий
Microsoft Visual Studio (рекомендуется 2019/2022)
.NET Framework 4.7.2 или 4.8 (WPF — проект тип .NET Framework, не .NET Core/5+) WPF (XAML + C#) — UI
WindowsFormsHost + System.Windows.Forms.DataVisualization.Charting — построение графиков (Chart).
WindowsFormsIntegration — хост WinForms-контролов в WPF.
Git (интеграция через VS) — для фиксации и пуша в удалённый репозиторий.

- Архитектура приложения
Проект организован как классическое WPF-приложение с навигацией:
/Практическая_Работа_4_Андреев_Маринин
  App.xaml
  MainWindow.xaml            // содержит меню/кнопки навигации и Frame
  /Pages
    Page1.xaml (+ Page1.xaml.cs)   // формула t, график t(x), Image сверху
    Page2.xaml (+ Page2.xaml.cs)   // выбор функции (sinh, x^2, e^x), график, Image сверху
    Page3.xaml (+ Page3.xaml.cs)   // таблица x/y, график y(x), Image сверху
  /Resources
    Styles.xaml
  README.md

MainWindow содержит Frame — в нём подгружаются Page1, Page2, Page3.
На каждой странице: Image (вверху) для скриншота формулы — путь задаётся в коде (константа), пользователь изменить не может.
Графики реализованы через System.Windows.Forms.DataVisualization.Charting в WindowsFormsHost.
Валидация ввода делается через double.TryParse + показ MessageBox при ошибках.
Выход из приложения с подтверждением реализован в MainWindow.Closing.
(Соответствует требованиям методички по интерфейсу и проверкам).
