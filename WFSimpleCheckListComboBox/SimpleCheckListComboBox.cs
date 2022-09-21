using System;
using System.Drawing;
using System.Windows.Forms;

namespace WFSimpleCheckListComboBox
{
    /// <summary>
    /// Простой компонент совмещающий функциональность checklist и combobox в режиме dropdownlist.
    /// Подойдёт для случаев, когда быстро и некритично нужен подобный функционал.
    /// </summary>
    public partial class SimpleCheckListComboBox: UserControl
    {
        #region Поля, свойства
        private bool _isFocused = false;
        private bool _isPressed = false;
        private string _text = "";
        /// <summary>
        /// Текстовое представление выбранных элементов через разделитель <see cref="ValuesSeparator"/>.
        /// </summary>
        public new string Text => _text;
        
        /// <summary>
        /// Выбран ли хотя бы один элемент?
        /// </summary>
        public bool HasCheckedAny => CheckedItems.Count > 0;
        /// <summary>
        /// Разделитель выбранных значений, отображаемых в компоненте.
        /// </summary>
        public string ValuesSeparator { get; set; }
        /// <summary>
        /// Максимальное количество выбираемых элементов в выпадающем списке.
        /// </summary>
        public int MaxDropDownItems { get; set; } = 5;
        private InnerCheckList _dropdown;
        /// <summary>
        /// Для отображения использовать свойство объекта с именем, заданным здесь.
        /// </summary>
        public string DisplayMember
        {
            get { return _dropdown.List.DisplayMember; }
            set { _dropdown.List.DisplayMember = value; }
        }
        /// <summary>
        /// Коллекция объектов, которые доступны в компоненте.
        /// </summary>
        public CheckedListBox.ObjectCollection Items
        {
            get { return _dropdown.List.Items; }
        }
        /// <summary>
        /// Коллекция выбранных элементов.
        /// </summary>
        public CheckedListBox.CheckedItemCollection CheckedItems
        {
            get { return _dropdown.List.CheckedItems; }
        }
        /// <summary>
        /// Индексы выбранных элементов.
        /// </summary>
        public CheckedListBox.CheckedIndexCollection CheckedIndices
        {
            get { return _dropdown.List.CheckedIndices; }
        }
        /// <summary>
        /// Изменено ли значение?
        /// </summary>
        public bool ValueChanged
        {
            get { return _dropdown.ValueChanged; }
        }
        /// <summary>
        /// Обработчик срабатывающий на событие изменение состояния элемента.
        /// </summary>
        public event ItemCheckEventHandler ItemCheck;

        private EventHandler _onTextChanged;
        /// <summary>
        /// Обработчик срабатывающий на изменение <see cref="Text"/>.
        /// </summary>
        public new event EventHandler TextChanged
        {
            add
            {
                _onTextChanged += value;
            }
            remove
            {
                _onTextChanged -= value;
            }
        }
        /// <summary>
        /// Обработчик срабатывающий на изменение <see cref="Text"/>.
        /// </summary>
        /// <param name="e"></param>
        protected new virtual void OnTextChanged(EventArgs e)
        {
            _onTextChanged?.Invoke(this, e);
        }
        #endregion Поля, свойства

        public SimpleCheckListComboBox() : base()
        {
            InitializeComponent();
            ValuesSeparator = ", ";
            _dropdown = new InnerCheckList(this);
            SetText("");            
        }

        //TODO: Фокус и клавиатура

        /// <summary>
        /// Состояние выбора элемента по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetItemChecked(int index)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                return _dropdown.List.GetItemChecked(index);
            }
        }
        /// <summary>
        /// Установка состояния выбора элемента по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isChecked"></param>
        public void SetItemChecked(int index, bool isChecked)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                _dropdown.List.SetItemChecked(index, isChecked);
                // После изменения состояния нужно обновление текста руками вызвать
                SetText(_dropdown.GetCheckedItemsAsString());
            }
        }
        /// <summary>
        /// Состояние элемента по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CheckState GetItemCheckState(int index)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                return _dropdown.List.GetItemCheckState(index);
            }
        }
        /// <summary>
        /// Установка состояния элемента по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <param name="state"></param>
        public void SetItemCheckState(int index, CheckState state)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                _dropdown.List.SetItemCheckState(index, state);
                // После изменения состояния нужно обновление текста руками вызвать
                SetText(_dropdown.GetCheckedItemsAsString());
            }
        }
        /// <summary>
        /// Заполнение текстового представления выбранных элементов.
        /// </summary>
        /// <param name="text"></param>
        internal void SetText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _text = text;
            }
            else
            {
                _text = Properties.Resources.TEXT_MESSAGE_NOT_CHECKED;
            }

            OnTextChanged(null);
            
            Refresh();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                // TODO: Сначала фокус поймать и не пущщать надо.
                // Обрабатывается только нажатие на кнопку "вниз".
                OnMouseClick(null);
            }
            // Спецклавиши и прочие отдельные не обрабатываем.            
            e.Handled = !e.Alt && !(e.KeyCode == Keys.Tab) &&
                        !((e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Home) || (e.KeyCode == Keys.End));

            base.OnKeyDown(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            _isPressed = false;
            base.OnMouseClick(e);

            // Установка размеров выпадающего списка согласно заданного значения максимально отображаемого количества элементов.
            if (!_dropdown.Visible)
            {
                Rectangle rect = RectangleToScreen(this.ClientRectangle);
                _dropdown.Location = new Point(rect.X, rect.Y + this.Size.Height);
                int count = _dropdown.List.Items.Count;
                if (count > this.MaxDropDownItems)
                {
                    count = this.MaxDropDownItems;
                }
                else
                {
                    if (count == 0)
                    {
                        count = 1;
                    }
                }
                _dropdown.Size = new Size(this.Size.Width, (_dropdown.List.ItemHeight) * count + 2);
                _dropdown.Show(this);
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isPressed = true;
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                RefreshOnMDown();
            }            
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _isFocused = true;
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            _isFocused = false;
        }

        public override void Refresh()
        {            
            base.Refresh();
        }
        public void RefreshOnMDown()
        {
            this.Refresh();
        }

        #region Отрисовка
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawNormalState(e);
        }

        private void DrawNormalState(PaintEventArgs e)
        {
            // Магия отрисовки компонента так, как хочется. "Я - художник, я так вижу" (ц).

            Brush blackBrush = new SolidBrush(Color.Black);
            Brush commonBrush = new SolidBrush(SystemColors.Control);

            Pen bgPen = new Pen(this.BackColor);
            Pen commonPen = new Pen(SystemColors.ControlDarkDark);
            Pen common2Pen = new Pen(SystemColors.ControlDark);
            Pen common3Pen = new Pen(SystemColors.Control);

            if (_isFocused)
            {
                bgPen = new Pen(SystemColors.GrayText);
            }

            if (!string.IsNullOrEmpty(this.Text))
            {
                int textX = 2;
                int textY = e.ClipRectangle.Height / 2 - (this.Font.Height / 2);
                Point p = new Point(textX, textY);
                SolidBrush frcolor = new SolidBrush(this.ForeColor);
                e.Graphics.DrawString(this.Text, this.Font, frcolor, p);
                frcolor.Dispose();
            }

            if (_isPressed)
            {
                e.Graphics.DrawRectangle(common2Pen, e.ClipRectangle.Top, e.ClipRectangle.Left, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
                e.Graphics.DrawRectangle(common3Pen, e.ClipRectangle.Top + 1, e.ClipRectangle.Left + 1, e.ClipRectangle.Width - 3, e.ClipRectangle.Height - 3);

                e.Graphics.DrawLine(bgPen, e.ClipRectangle.Width - 21, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 21, e.ClipRectangle.Height - 3);
                e.Graphics.DrawLine(bgPen, e.ClipRectangle.Width - 22, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 22, e.ClipRectangle.Height - 3);
                e.Graphics.DrawLine(common2Pen, e.ClipRectangle.Width - 20, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 20, e.ClipRectangle.Height - 3);
                e.Graphics.DrawLine(common3Pen, e.ClipRectangle.Width - 19, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 19, e.ClipRectangle.Height - 3);
            }
            else
            {
                e.Graphics.DrawRectangle(commonPen, e.ClipRectangle.Top, e.ClipRectangle.Left, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
                e.Graphics.DrawRectangle(common2Pen, e.ClipRectangle.Top + 1, e.ClipRectangle.Left + 1, e.ClipRectangle.Width - 3, e.ClipRectangle.Height - 3);
                
                e.Graphics.DrawLine(bgPen, e.ClipRectangle.Width - 21, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 21, e.ClipRectangle.Height - 3);
                e.Graphics.DrawLine(bgPen, e.ClipRectangle.Width - 22, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 22, e.ClipRectangle.Height - 3);
                e.Graphics.DrawLine(commonPen, e.ClipRectangle.Width - 20, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 20, e.ClipRectangle.Height - 3);
                e.Graphics.DrawLine(common2Pen, e.ClipRectangle.Width - 19, e.ClipRectangle.Top + 2, e.ClipRectangle.Width - 19, e.ClipRectangle.Height - 3);
            }            

            int x_offset = e.ClipRectangle.Width - 18;
            e.Graphics.FillPolygon(commonBrush, new Point[] {
                new Point(x_offset, e.ClipRectangle.Top + 2),
                new Point(x_offset + 16, e.ClipRectangle.Top + 2),
                new Point(x_offset + 16, e.ClipRectangle.Height - 2),
                new Point(x_offset, e.ClipRectangle.Height - 2)
            });

            x_offset = e.ClipRectangle.Width - 15;
            int y_offset = e.ClipRectangle.Height / 2 - 2;
            e.Graphics.FillPolygon(blackBrush, new Point[] {
                new Point(x_offset + 0,0 + y_offset),
                new Point(x_offset + 10,0 + y_offset),
                new Point(x_offset + 5,5 + y_offset),
                new Point(x_offset + 1,0 + y_offset) }
            );

            blackBrush.Dispose();
            bgPen.Dispose();
            commonPen.Dispose();
            common2Pen.Dispose();
            common3Pen.Dispose();
            commonBrush.Dispose();
        }

        #endregion Отрисовка

    }
}
