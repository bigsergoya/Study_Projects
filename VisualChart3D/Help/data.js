﻿var contents = new Array("\n\r\n\rДля просмотра информации об объектах следует открыть окно в пункте меню&nbsp;Вид -&gt; Список объектов. После этого, при выделении объектов в открытом окне будет отображаться информация о них. Для выделения объектов следует с зажатой правой кнопкой мыши нарисовать прямоугольник. Пример показан на рисунке 12         \n\r\n\r\n\rРисунок 12 - Выделение объектов                                                \n\r\n\r                                                \n\r\n\rВ программе предусмотрена функция построение выделенных объектов независимо от остальных. Для этого после выделения объектов следует&nbsp;сделать двойной щелчок левой кнопкой мыши. Для выхода из этого режима следует нажать клавишу backspace. Внизу экрана представлена информация, в каком&nbsp;режиме находиться визуализация:\n\r  Windows: Main - &nbsp;происходит визуализация   основных данных   Windows: Subsidiary - происходит визуализация дочернего участка. \n\r\n\r\n\rПример на рисунке 13\n\r\n\r\n\r\n\rРисунок 13 - Построение локального участка","Выделение объектов","scr\\Выделение_объектов.htm","\n\r\n\rИсходная матрица может быть представлена в двух вариантах: матрица расстояний и матрица объект-признак         \n\r\n\rМатрица расстояний                     \n\r\n\rМатрица расстояний представляет из себя матрицу NxN, где N - количество объектов. Разделение по столбцам должно быть либо табуляцией, либо пробелом. Цифры могут быть представлены как целыми числами, так и десятичными. Пример исходной матрицы расстояний представлен на рисунке 1.\n\rВажно! Если разделитель в десятичных числах используется не тот, что задан в системе, то файл возможно будет считываться дольше, так как требуется дополнительные преобразования.                     \n\r\n\r&nbsp;\n\rРисунок 1 - Пример матрицы расстояний                     \n\r\n\r                     \n\r\n\rДля задания исходной матрицы расстояний требуется в окне \"Настройки исходных данных\" выбрать пункт \"Матрица расстояний\", нажать на кнопку \"Обзор\"&nbsp;и выбрать соответствующий файл. На рисунке 2 представлен пример                      \n\r\n\r\n\rРисунок&nbsp;2 -&nbsp;Выбор матрицы расстояний&nbsp;                     \n\r\n\rМатрица \"объект-признак\"                     \n\r\n\rМатрица \"объект-признак\" представляет из себя матрицу NxM, где N - количество объектов, M - количество признаков. Разделение по столбцам должно быть либо табуляцией, либо пробелом. Цифры могут быть представлены как целыми числами, так и десятичными. Пример исходной матрицы расстояний представлен на рисунке 3.\n\rВажно! Если разделитель в десятичных числах используется не тот, что задан в системе, то файл возможно будет считываться дольше, так как требуется дополнительные преобразования.                     \n\r\n\r&nbsp;\n\rРисунок&nbsp;3 - Пример матрицы \"объект-признак\"                     \n\r\n\r                     \n\r\n\rДля задания исходной матрицы \"объект-признак\" требуется в окне \"Настройки исходных данных\" выбрать пункт \"Матрица объект-признак\", нажать на кнопку \"Обзор\"&nbsp;и выбрать соответствующий файл. На рисунке&nbsp;4 представлен пример                      \n\r\n\r\n\rРисунок&nbsp;4 -&nbsp;Выбор матрицы \"объект-признак\"                     ","Исходная матрица","scr\\Исходная_матрица_.htm","\n\r\n\rИсходные данные должны быть представлены в виде текстовых файлов. Для выбора исходных файлов требуется&nbsp;выбрать пункт меню Файл -&gt;&nbsp;Исходные данные. В программе можно указать три файла с исходными данными         \n\r  Исходная матрица     (обязательный файл)           Файл с   классами объектов     (необязательный   файл)           Файл с именами   объектов     (необязательный   файл)         \n\r","Исходные данные","scr\\Исходные_данные.htm","\n\r\n\rВ программе можно изменить внешний вид&nbsp;для каждого класса объектов. В случае, если файл с классами объектов не был задан, считается, что все объекты принадлежат одному классу. Для изменения внешнего вида после задания исходных данных требуеться перейти в пункт меню Настройки -&gt; Настройка классов. Пример появившегося окна представлен на рисунке 11         \n\r\n\r\n\rРисунок 11 - Окно настройки классов                                                         \n\r\n\r В данном окне можно настроить размер объектов и&nbsp;количество полигонов для всех объектов. Для каждого класса можно настроить цвет и                                                форму.         \n\r\n\rПримечание! После сохранения настроек, в папке, где находиться файл с классами объектов, будет создан файл имя_класса_с_объектами.clrSchema. В данном файле сохраняются настройки внешнего вида                                                классов.         ","Настройка классов","scr\\Настройка_классов.htm","\n\r\n\r&nbsp;Visual Chart 3D - программа для визуализации в трёхмерном пространстве данных, представленных в виде матрицы расстояний                  \n\r\n\r         Ключевые особенности Visual Chart 3D:                  \n\r           Исходные данные могут быть   представлены как матрицей расстояний, так и матрицей \"объект-признак\"                             Возможность задания классов   объектов тремя способами: один к одному, по количеству объектов или с начала   класса                             Возможность задания имён   объектам                             Настройка внешнего вида   объектов каждого класса&nbsp;                             Сохраняемость настроек                             Высокая скорость работы         \n\r\n\r\n\r                    \n\r\n\r                    ","Общая информация","scr\\Общая_информация.htm","\n\r\n\rОбщая информация     \n\r\n\rРабота с программой            \n\r  Исходные данные   \n\r            \n\r  Исходная     матрица \n\r  Файл с классами объктов \n\r  Файл с именами     объектов\n\r\n\r\n\r          \n\r  Настройка   классов \n\r            Управление камерой \n\r            Выделение объектов             \n\r","Содержание","scr\\Содержание.htm","\n\r\n\r   В данном разделе описано&nbsp;управление камерой \n\r                \n\r\n\rМасштабирование\n\rДля масштабирования используются либо колесо прокрутки, либо клавиши \'+\' и \'-\';         \n\r\n\r           \n\rВращение\n\rДля вращения необходимо зажать левую кнопку мыши и не отпуская её переместить мышь в нужном направлении.         \n\r\n\r                \n\rПеремещение\n\rДля перемещения камеры требуется зажать клавишу Shift и левую кнопку мыши, а далее переместить мышь не отпуская зажатых клавиш.         \n\r\n\r               \n\rВосстановление исходного положения   \n\rДля восстановления исходного положения камеры требуется нажать клавишу Home         \n\r\n\r                \n\r\n\r                 ","Управление камерой","scr\\Управление_камерой.htm","\n\r\n\r  По желанию можно задать файл с именами объектов. Данный файл представляет собой матрицу Nx1, где N - количество объектов. Пустые строки игнорируются. В данном файле в столбец записывается название для каждого объекта по порядку. Пример представлен на рисунке 9 \n\r\n\r\n\r   Рисунок&nbsp;9 - Пример файла с именами объектов \n\r\n\r \n\r\n\r     Для     задания&nbsp;файла с именами объектов&nbsp;требуется в окне \"Настройки исходных данных\" выбрать пункт \"Файл с именами объектов\",&nbsp;и&nbsp;&nbsp;нажать на кнопку \"Обзор\"&nbsp;и выбрать соответствующий файл. На рисунке&nbsp;10 представлен пример\n\r\n\r\n\rРисунок&nbsp;10 -&nbsp;Выбор файла с именами объектов ","Файл с именами объектов","scr\\Файл_с_именами_объектов.htm","\n\r\n\rПо желанию можно задать файл с классами объектов. Данный файл может быть представлен в трех видах.         \n\r\n\rОдин-к-одному\n\r Данный файл представляет собой матрицу Nx1, где N - количество объектов. В данном файле в столбец записывается название класса для каждого объекта по порядку. Пример представлен на рисунке 5         \n\r\n\r\n\rРисунок 5 - Пример файла с классами объектов типа \"один-к-одному\"         \n\r\n\rЧисло объектов класса\n\r                            Данный файл может быть представлен двумя видами:         \n\r  матрица Mx2, где M -                       количество классов. В первый столбец записывается количество объектов, во второй название класса.           матрица                       Mx1, где M - количество классов. В первый столбец записывается количество объектов         \n\r\n\r\n\r            \n\r                       &nbsp;Разделение по столбцам либо пробел, либо табуляция. Пример представлен на рисунке 6.         \n\r\n\r&nbsp; \n\rРисунок&nbsp;6 - Пример файла с классами объектов типа&nbsp;\"число объектов класса\"         \n\r\n\rНачало класса                                              \n\r\n\r                               Данный файл может быть представлен двумя видами:         \n\r  матрица Mx2, где M - количество классов. В первый столбец записывается порядковый                       номер объекта (начиная с нуля) с которого начинается класс,&nbsp;во второй название класса.           матрица Mx1, где M - количество классов. В первый                       столбец записывается порядковый номер объекта (начиная с нуля) с которого начинается класс         \n\r\n\r\n\r \n\r                          &nbsp;Разделение по столбцам либо пробел, либо табуляция. Пример представлен на рисунке 7.         \n\r\n\r&nbsp;&nbsp;\n\rРисунок&nbsp;7 - Пример файла с классами объектов типа&nbsp;\"начало класса\"         \n\r\n\rДля задания&nbsp;файла с классами объектов&nbsp;требуется в окне \"Настройки исходных данных\" выбрать пункт \"Файл с классами объектов\", выбрать нужный тип, &nbsp;нажать на кнопку \"Обзор\"&nbsp;и выбрать соответствующий файл. На рисунке&nbsp;8 представлен пример\n\r\n\r\n\r   Рисунок&nbsp;8 -&nbsp;Выбор файла с классами объектов         \n\r\n\r Если число объектов в каждом классе одинаково, можно задать его числом, как показано на рисунке 9     \n\r\n\r      \n\r\n\rРисунок&nbsp;9 -&nbsp;Указание число объектов в классе      \n\r\n\r &nbsp;     ","Файл с классами объектов","scr\\Файл_с_классами_объектов_.htm");