### Exemplo de integração NLua com Godot C#

Abaixo tem um exemplo completo e prático que mostra:
- Como criar um LuaState com NLua;
- Como expor um proxy seguro de um Node Godot ao Lua;
- Como carregar e executar um script Lua que altera propriedades em tempo de execução;
- Observações sobre empacotamento das DLLs nativas e boas práticas de sandboxing.

Siga a ordem: instalar NLua, adicionar DLLs nativas, criar scripts C# no Godot, testar e empacotar.

---

### 1. Instalação e dependências
- Adicione NLua ao seu projeto C# do Godot (Mono). Recomendo usar o pacote NuGet NLua (ou adicionar a DLL manualmente em addons/libs).
- Inclua KeraLua/KeraLua native binaries compatíveis para cada plataforma alvo (Windows x86/x64, Linux x86/x64/ARM, Android ARM64 etc.). Coloque-os em um diretório que será empacotado junto ao jogo (por exemplo, res://libs/nlua/ ou em build pipeline).
- No Godot, assegure-se de que o assembly NLua.dll e as native libs estejam acessíveis ao runtime Mono.

---

### 2. Estrutura de arquivos sugerida
- res://Scripts/LuaController.cs         (controlador que executa scripts)
- res://Scripts/PlayerProxy.cs          (proxy seguro expondo propriedades)
- res://LuaScripts/tweak.lua            (exemplo de script editável em runtime)
- res://libs/nlua/                      (contém NLua.dll + native lib por plataforma)

---

### 3. Código: PlayerProxy.cs
```csharp
using Godot;

public class PlayerProxy
{
    private Node _player;

    public PlayerProxy(Node player)
    {
        _player = player;
    }

    // Exponha apenas o necessário; propriedades simples e seguras.
    public float PositionX
    {
        get
        {
            var pos = ((Node2D)_player).Position;
            return pos.x;
        }
        set
        {
            var node = (Node2D)_player;
            var pos = node.Position;
            pos.x = value;
            node.Position = pos;
        }
    }

    public int Health
    {
        get => (int)(_player.Get("health") ?? 100);
        set => _player.Set("health", value);
    }

    // Métodos seguros para interações limitadas
    public void Print(string msg)
    {
        GD.Print("[Lua] ", msg);
    }
}
```

---

### 4. Código: LuaController.cs
```csharp
using Godot;
using System;
using System.IO;
using NLua; // Namespace NLua

public class LuaController : Node
{
    private Lua _lua;
    private PlayerProxy _playerProxy;

    public override void _Ready()
    {
        // Inicializa o estado Lua
        _lua = new Lua();
        _lua.State.Encoding = System.Text.Encoding.UTF8;

        // Abre bibliotecas essenciais se desejar; cuidado com open libs por segurança
        _lua.DoString("math = math; string = string; table = table");

        // Exponha proxies/objetos C# para o ambiente Lua
        var playerNode = GetNode<Node2D>("Player");
        _playerProxy = new PlayerProxy(playerNode);
        _lua["player"] = _playerProxy;

        // Carrega e executa script de exemplo
        var scriptPath = "res://LuaScripts/tweak.lua";
        var luaCode = LoadScriptText(scriptPath);
        ExecuteLua(luaCode);
    }

    private string LoadScriptText(string path)
    {
        var abs = ProjectSettings.GlobalizePath(path);
        return File.ReadAllText(abs);
    }

    private void ExecuteLua(string code)
    {
        try
        {
            _lua.DoString(code);
            GD.Print("Lua executed successfully");
        }
        catch (Exception ex)
        {
            GD.PrintErr("Lua execution error: ", ex.Message);
        }
    }

    public void RunLuaFromString(string code)
    {
        ExecuteLua(code);
    }

    public override void _ExitTree()
    {
        _lua?.Dispose();
        _lua = null;
    }
}
```

---

### 5. Exemplo de script Lua (res://LuaScripts/tweak.lua)
```lua
-- tweak.lua
-- Exemplo simples que ajusta X e saúde
player:Print("Initial X = " .. tostring(player.PositionX))
player.PositionX = player.PositionX + 50
player.Health = player.Health - 10
player:Print("New X = " .. tostring(player.PositionX) .. " Health = " .. tostring(player.Health))
```

---

### 6. Boas práticas de segurança e estabilidade
- Nunca exponha o Node Godot diretamente; sempre use proxies que limitem métodos/propriedades disponíveis.
- Evite abrir todas as bibliotecas padrão do Lua. Abra apenas as que precisa (math, string, table) e remova io/os se não necessário.
- Execute scripts potencialmente não confiáveis em um processo separado quando possível; se isso não for viável, implemente limites de instruções (contador de instruções) ou timeouts externos.
- Trate exceções de NLua e sempre limpar/descartar o LuaState ao sair.
- Se o script puder alterar muitos elementos por frame, controle a frequência (ex.: só rodar em eventos ou a cada N frames).

---

### 7. Considerações sobre empacotamento e exportação
- Inclua NLua.dll no diretório de assemblies do projeto Godot (ex.: Bin/ ou Assemblies/). No Godot Mono, assemblies customizados vão dentro de Bin/ por padrão.
- As native libs (KeraLua / lua binary) devem ficar no mesmo diretório onde o runtime espera carregar (ou no PATH / LD_LIBRARY_PATH em Linux). Para Android, empacote as bibliotecas .so no APK em libs/armeabi-v7a, libs/arm64-v8a etc., conforme o formato do Godot export template.
- Em builds IL2CPP/AOT, confirme compatibilidade: NLua depende de chamadas nativas, então teste o pipeline de exportação; consoles e certas plataformas podem restringir execução de código nativo dinâmico.
- Teste cada plataforma cedo: Windows x64, Linux, Android, iOS (se aplicável), Web (nota: WebGL geralmente não aceita binários nativos, dificultando uso de NLua).

---

### 8. Testes e diagnóstico
- Teste com scripts simples e verifique logs GD.Print.  
- Faça um build de desenvolvimento com símbolos e rode no dispositivo alvo para validar carregamento das DLLs nativas.  
- Se a biblioteca nativa não carregar, mensagens de erro do SO/Mono ajudarão (falta de dependência, arquitetura errada, caminho incorreto).

---

### 9. Checklist rápido antes do deploy
1. NLua.dll presente no build final.  
2. Binaries nativos (KeraLua/.so/.dll) para cada arquitetura incluídos e no caminho correto.  
3. Proxies implementados para limitar superfície API.  
4. Scripts testados em todos os alvos (AOT/JIT diferenças).  
5. Estratégia para timeouts e tratamento de erros definida.

---

Se quiser, eu adapto esse exemplo para:
- incluir um contador de instruções/timeout no ambiente Lua;  
- demonstrar carregamento dinâmico de scripts a partir de um editor in-game;  
- gerar um passo a passo detalhado de empacotamento das libraries nativas para Windows, Linux e Android. Escolha qual desses quer agora e eu gero o código/guia correspondente.
