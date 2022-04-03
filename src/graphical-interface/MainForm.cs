using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace graphical_interface {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void textHead_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char) Keys.Back);
        }

        private void textTail_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char) Keys.Back);
        }

        private void checkHead_CheckedChanged(object sender, EventArgs e) {
            textHead.Enabled = checkHead.Enabled & checkHead.Checked;
        }

        private void checkTail_CheckedChanged(object sender, EventArgs e) {
            textTail.Enabled = checkHead.Enabled & checkTail.Checked;
        }

        private void radioAll_CheckedChanged(object sender, EventArgs e) {
            if (radioAll.Checked || radioDiffHeadMaxWords.Checked) {
                checkHead.Enabled = false;
                checkTail.Enabled = false;
                checkLoop.Enabled = false;
            } else {
                checkHead.Enabled = true;
                checkTail.Enabled = true;
                checkLoop.Enabled = true;
            }
        }

        private void radioDiffHeadMaxWords_CheckedChanged(object sender, EventArgs e) {
            if (radioAll.Checked || radioDiffHeadMaxWords.Checked) {
                checkHead.Enabled = false;
                checkTail.Enabled = false;
                checkLoop.Enabled = false;
            } else {
                checkHead.Enabled = true;
                checkTail.Enabled = true;
                checkLoop.Enabled = true;
            }
        }

        private void checkHead_EnabledChanged(object sender, EventArgs e) {
            textHead.Enabled = checkHead.Enabled & checkHead.Checked;
        }

        private void checkTail_EnabledChanged(object sender, EventArgs e) {
            textTail.Enabled = checkHead.Enabled & checkTail.Checked;
        }

        private void buttonRead_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt";

            if (DialogResult.OK == dialog.ShowDialog()) {
                string path = dialog.FileName;

                if (!File.Exists(path)) {
                    MessageBox.Show("无法找到文件", "发生错误", MessageBoxButtons.OK);
                }

                try {
                    var rawInput = File.ReadAllText(path);
                    var wordList = Lexer.Lex(rawInput).ToArray();
                    textWords.Text = string.Join("\n", wordList);
                } catch (IOException) {
                    MessageBox.Show("无法读取文件", "发生错误", MessageBoxButtons.OK);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            if (DialogResult.OK == dialog.ShowDialog()) {
                string path = dialog.FileName;
                File.WriteAllText(path, string.Join("\n", textChains));
            }
        }

        private void buttonRun_Click(object sender, EventArgs e) {
            try {
                var wordList = Lexer.Lex(textWords.Text);
                List<string> result;
                var retStr = new StringBuilder();

                if (radioAll.Checked) {
                    result = CoreCaller.GenChainsAll(wordList);
                } else if (radioDiffHeadMaxWords.Checked) {
                    result = CoreCaller.GenChainWordUnique(wordList);
                } else if (radioMaxWords.Checked) {
                    result = CoreCaller.GenChainWord(wordList,
                        checkHead.Checked && textHead.Text != "" ? textHead.Text[0] : '\0',
                        checkTail.Checked && textTail.Text != "" ? textTail.Text[0] : '\0',
                        checkLoop.Checked
                    );
                } else {
                    result = CoreCaller.GenChainChar(wordList,
                        checkHead.Checked && textHead.Text != "" ? textHead.Text[0] : '\0',
                        checkTail.Checked && textTail.Text != "" ? textTail.Text[0] : '\0',
                        checkLoop.Checked
                    );
                }

                result.ForEach(line => retStr.AppendLine(line));
                textChains.Text = retStr.ToString();
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "发生错误", MessageBoxButtons.OK);
            }
        }
    }
}
