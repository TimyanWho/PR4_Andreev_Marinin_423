# Поддержка и тестирование программных модулей
# ПРАКТИЧЕСКАЯ РАБОТА #4 ТЕСТИРОВАНИЕ "БЕЛЫМ ЯЩИКОМ"

Цель работы: приобрести практические навыки ручного тестирования
методом "белого ящика".

## Маринин Степан и Андреев Тимофей, 3ИСИП-423

## Варианты задания со скриншотами функций:
<img width="351" height="87" alt="f1" src="https://github.com/user-attachments/assets/0d6e0d52-573f-4603-823e-d62bc73b25a6" />

<img width="414" height="110" alt="f2" src="https://github.com/user-attachments/assets/069ff42f-8e63-44c2-bb88-51302e6d9096" />

<img width="375" height="119" alt="f3" src="https://github.com/user-attachments/assets/ea6a96a2-d464-4b38-b3e7-1bbbddb55da3" />

## Cтек технологий
Microsoft Visual Studio (рекомендуется 2019/2022)

.NET Framework 4.7.2 или 4.8 (WPF — проект тип .NET Framework, не .NET Core/5+) WPF (XAML + C#) — UI

WindowsFormsHost + System.Windows.Forms.DataVisualization.Charting — построение графиков (Chart).

WindowsFormsIntegration — хост WinForms-контролов в WPF.

Git (интеграция через VS) — для фиксации и пуша в удалённый репозиторий.

## Архитектура приложения
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



# Практическая работа №6 — Часть 2  
## Маринин Степан и Андреев Тимофей, 3ИСИП-423

**Тема:** Рефакторинг вычислительной логики и покрытие unit-тестами  
**Проект:** Практическая_Работа_4_Андреев_Маринин

---

## 1. Цель работы
Вынести вычислительную логику из UI (Page*.xaml.cs) в отдельный тестируемый модуль, добавить XML-комментарии, написать unit-тесты (MSTest) для ключевых сценариев и продемонстрировать прохождение тестов.

---

## 2. Что было сделано (коротко)
- Создан новый файл `Calculations.cs` (namespace тот же, что и у проекта): в нём находятся статические методы:
  - `bool TryComputeT(double x, double y, double z, out double t)`
  - `bool TryComputeA(double x, double y, int functionType, out double a)`
  - `bool TryComputeY(double x, double aParam, double b, double c, out double y)`
- Методы возвращают `true` при успешном вычислении (и корректном числе), иначе `false`. Такая сигнатура упрощает тестирование и обработку ошибок в UI.
- UI-обработчики (`Page1`, `Page2`, `Page3`) переписаны: они парсят ввод и вызывают соответствующие `Calculations.Try...`. UI теперь не содержит «чистой математики».
- Добавлен тестовый проект (MSTest, .NET Framework) `UnitTests_Practice6` с набором тестов, покрывающих основные ветви вычислений и кейсы ошибок (деление на ноль, sqrt отрицательного аргумента и т.п.).
- Добавлены XML-комментарии к методам в `Calculations.cs`.
- <img width="1061" height="445" alt="{9409EFB9-5F1E-451B-9B53-A96111801B1C}" src="https://github.com/user-attachments/assets/cbb2a620-7d27-4df4-b94f-26851325a56e" />


---

## 3. Структура (файлы, которые изменены / добавлены)
```
/Практическая_Работа_4_Андреев_Маринин
Calculations.cs // новый: бизнес-логика (Testable)
/Pages
Page1.xaml/.xaml.cs // вызывает Calculations.TryComputeT
Page2.xaml/.xaml.cs // вызывает Calculations.TryComputeA
Page3.xaml/.xaml.cs // вызывает Calculations.TryComputeY
/UnitTests_Practice6 // новый проект (MSTest, .NET Framework)
UnitTest1.cs // тесты для TryComputeT/TryComputeA/TryComputeY
README_PRACTICE6.md // этот файл (описание и инструкции)
/docs/practice6/screenshots/
```


---

## 4. Как собрать и запустить (локально)
> Рекомендуется использовать Visual Studio 2019/2022.

### Сборка приложения
1. Откройте решение `Практическая_Работа_4_Андреев_Маринин.sln` в Visual Studio.  
2. Убедитесь, что главный проект таргетит **.NET Framework 4.7.2/4.8**.  
3. Rebuild Solution (Build → Rebuild Solution).

### Запуск приложения
- F5 или Debug → Start Debugging.  
- Проверьте страницы: Page1 (вычисление `t`), Page2 (вычисление `a`), Page3 (таблица + график).

### Запуск unit-тестов
- Откройте Test Explorer (Test → Test Explorer).  
- Нажмите Run All (или выберите нужные тесты).  
- Результаты тестов появятся в Test Explorer.

(Если хотите запускать тесты из консоли в Windows — используйте `vstest.console.exe` из Visual Studio tools с указанием `.trx` или dll тестов. Для практики достаточно Test Explorer.)

---

## 5. Описание тестов (что проверяется)
В тестах должны быть проверки для следующих типов сценариев:
1. **Успешное вычисление T** — адекватные x,y,z дают `TryComputeT` = `true` и конечное числовое значение.  
2. **Пограничные/негативные случаи T** — например, внутренний делитель становится нулём → `TryComputeT` = `false`.  
3. **Ветвления `TryComputeA`** — проверки для `xy > 0`, `xy < 0`, `xy == 0`. В частности:
   - случай `xy > 0` и `f*y < 0` должен возвращать `false` (sqrt отрицательного в этой ветке);  
   - корректные значения дают `true` и ожидаемый числовой результат (по заранее рассчитанному примеру).  
4. **Деление на ноль в `TryComputeY`** — `x == 0` → `false`.  
5. Дополнительные кейсы: NaN/Infinity проверки, большие/малые значения.

