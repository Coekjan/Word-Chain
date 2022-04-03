# 项目构建产物相关

本项目采用 Github Actions 对代码进行单元测试、回归测试，并生成最新版代码的构建产物。若要获取本项目最后一个版本的构建产物，请前往本仓库的 Actions 页面里最新一次成功的 Workflow 中下载构建产物 Word-Chain-Artifacts。其中包含：
- core.dll：程序核心，用于计算单词链。动态链接库，可用于对接用户接口。
- WordChainCLI.exe：命令行接口程序。
- WordChainGUI.exe：图形接口程序。
