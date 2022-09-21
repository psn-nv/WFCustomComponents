using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFSimpleCheckListComboBox
{
    partial class SimpleCheckListComboBox
    {
        /// <summary>
        /// Внутренний класс <see cref="SimpleCheckListComboBox"/> отвечающий за отображение и работу с "выпадающим списком"
        /// </summary>
        internal class InnerCheckList : Form
        {
            #region Вспомогательные классы
            /// <summary>
            /// Класс для аргументов обработки событий компонента
            /// </summary>
            internal class ItemsBoxEventArgs : EventArgs
            {
                public bool AssignValues { get; set; }
                public EventArgs EventArgs { get; set; }
                public ItemsBoxEventArgs(EventArgs e, bool assignValues) : base()
                {
                    EventArgs = e;
                    AssignValues = assignValues;
                }
            }

            /// <summary>
            /// Чеклист для показа выпадающего списка
            /// </summary>
            internal class InternalSimpleCheckedListBox : CheckedListBox
            {
                private int currentSelectionIndex = -1;
                public InternalSimpleCheckedListBox() : base()
                {
                    this.SelectionMode = SelectionMode.One;
                    this.HorizontalScrollbar = true;
                }

                /// <summary>
                /// Обработка нажатий клавиш при работе с "выпадающим списком".
                /// </summary>
                /// <param name="e"></param>
                protected override void OnKeyDown(KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        // Подтвердить выбор.
                        ((SimpleCheckListComboBox.InnerCheckList)Parent).OnDeactivate(new ItemsBoxEventArgs(null, true));
                        e.Handled = true;
                    }
                    else
                    {
                        if (e.KeyCode == Keys.Escape)
                        {
                            // Отменить выбор.
                            ((SimpleCheckListComboBox.InnerCheckList)Parent).OnDeactivate(new ItemsBoxEventArgs(null, false));
                            e.Handled = true;
                        }
                        else
                        {
                            if (e.KeyCode == Keys.Delete)
                            {
                                // По "Delete" отмена выбора всех элементов. По "Shift"+"Delete" - выбор всех элементов.
                                for (int i = 0; i < Items.Count; i++)
                                {
                                    SetItemChecked(i, e.Shift);
                                }
                                e.Handled = true;
                            }
                        }
                    }
                    // Все другие клавиши обрабатываются родительским объектом.
                    base.OnKeyDown(e);
                }

                protected override void OnMouseMove(MouseEventArgs e)
                {
                    base.OnMouseMove(e);
                    int index = IndexFromPoint(e.Location);
                    
                    if ((index >= 0) && (index != currentSelectionIndex))
                    {
                        currentSelectionIndex = index;
                        SetSelected(index, true);
                    }
                }
            }
            #endregion Вспомогательные классы

            private SimpleCheckListComboBox _parentChckCmbBox;

            /// <summary>
            /// Понадобится для определения факта изменения значения.
            /// </summary>
            private string _oldStrValue = "";
            public bool ValueChanged
            {
                get
                {
                    string newStringValue = _parentChckCmbBox.Text;
                    if ((_oldStrValue.Length > 0) && (newStringValue.Length > 0))
                    {
                        return (_oldStrValue.CompareTo(newStringValue) != 0);
                    }
                    else
                    {
                        return (_oldStrValue.Length != newStringValue.Length);
                    }
                }
            }

            /// <summary>
            /// Копия текущих выбранных на случай если пользователь отменит изменения.
            /// </summary>
            private bool[] _checkedStateArr;
            private bool _isDropdownClosed = true;

            private InternalSimpleCheckedListBox _internalChckLstBox;
            public InternalSimpleCheckedListBox List
            {
                get { return _internalChckLstBox; }
                set { _internalChckLstBox = value; }
            }

            public InnerCheckList(SimpleCheckListComboBox parentCCB)
            {
                this._parentChckCmbBox = parentCCB;
                InitializeComponent();
                this.ShowInTaskbar = false;
                // Непосредственно компонент тоже нужно оповещать об изменениях в "выпадающем листе".
                this._internalChckLstBox.ItemCheck += new ItemCheckEventHandler(this.ichcklst_ItemCheck);
            }

            /// <summary>
            /// Расчёт параметров и создание "выпадающего списка".
            /// </summary>
            private void InitializeComponent()
            {
                this._internalChckLstBox = new InternalSimpleCheckedListBox();
                this.SuspendLayout();

                #region _internalChckLstBox
                this._internalChckLstBox.BorderStyle = BorderStyle.None;
                this._internalChckLstBox.Dock = DockStyle.Fill;
                this._internalChckLstBox.FormattingEnabled = true;
                this._internalChckLstBox.Location = new System.Drawing.Point(0, 0);
                this._internalChckLstBox.Name = "_ichcklstbx";
                this._internalChckLstBox.Size = new System.Drawing.Size(47, 15);
                this._internalChckLstBox.TabIndex = 0;
                #endregion _internalChckLstBox

                #region InnerCheckList                
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = AutoScaleMode.Font;
                this.BackColor = System.Drawing.SystemColors.Menu;
                this.ClientSize = new System.Drawing.Size(47, 16);
                this.ControlBox = false;
                this.Controls.Add(this._internalChckLstBox);
                this.ForeColor = System.Drawing.SystemColors.ControlText;
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                this.MinimizeBox = false;
                this.Name = "parentCCB";
                this.StartPosition = FormStartPosition.Manual;
                this.ResumeLayout(false);
                #endregion InnerCheckList
            }
            /// <summary>
            /// Сбор текстового представления выбранных элементов.
            /// </summary>
            /// <returns></returns>
            public string GetCheckedItemsAsString()
            {
                StringBuilder sb = new StringBuilder("");
                for (int i = 0; i < _internalChckLstBox.CheckedItems.Count; i++)
                {
                    sb.Append(_internalChckLstBox.GetItemText(_internalChckLstBox.CheckedItems[i])).Append(_parentChckCmbBox.ValuesSeparator);
                }
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - _parentChckCmbBox.ValuesSeparator.Length, _parentChckCmbBox.ValuesSeparator.Length);
                }
                return sb.ToString();
            }

            protected override void OnActivated(EventArgs e)
            {                
                base.OnActivated(e);
                _isDropdownClosed = false;
                
                // Подготовка копий данных о состояних элементов на случай отмены изменений пользователем.
                _oldStrValue = _parentChckCmbBox.Text;
                _checkedStateArr = new bool[_internalChckLstBox.Items.Count];

                for (int i = 0; i < _internalChckLstBox.Items.Count; i++)
                {
                    _checkedStateArr[i] = _internalChckLstBox.GetItemChecked(i);
                }
            }

            protected override void OnDeactivate(EventArgs e)
            {
                base.OnDeactivate(e);
                ItemsBoxEventArgs ibEA = e as ItemsBoxEventArgs;
                if (ibEA != null)
                {
                    CloseDropdown(ibEA.AssignValues);
                }
                else
                {
                    // Если передан стандартный EventArgs, значит метод вызван из окружения.
                    // В этом случае сразу считаю изменения применившимися.
                    CloseDropdown(true);
                }
            }

            private void ichcklst_ItemCheck(object sender, ItemCheckEventArgs e)
            {
                if (_parentChckCmbBox.ItemCheck != null)
                {
                    _parentChckCmbBox.ItemCheck(sender, e);
                }
            }

            /// <summary>
            /// Закрытие "выпадающего списка" и применение или отмена внесенных изменений.
            /// </summary>
            /// <param name="isApplyChanges"></param>
            public void CloseDropdown(bool isApplyChanges)
            {
                if (_isDropdownClosed)
                {
                    return;
                }

                // Если изменения применяются
                if (isApplyChanges)
                {
                    // Текстовое представление в родительском компоненте актуализируется.
                    _parentChckCmbBox.SetText(GetCheckedItemsAsString());
                }
                else
                {
                    // Если изменения отменены - то восстанавливаются предыдущие состояния выбора для элементов.
                    for (int i = 0; i < _internalChckLstBox.Items.Count; i++)
                    {
                        _internalChckLstBox.SetItemChecked(i, _checkedStateArr[i]);
                    }
                }
                
                // Закрытие весьма условно - скрывается "выпадающий список", дабы каждый раз не производить его создание.
                // Но при скрытии опять сработает обработчик на деактивацию, а из него вызов текущего метода.
                // Вот переменная ниже и нужна для предотвращения зацикливания.
                _isDropdownClosed = true;
                // И не забыть вернуть фокус родительскому компоненту.
                _parentChckCmbBox.Focus();
                this.Hide();
            }
        }

    }
}
