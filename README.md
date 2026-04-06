# 🪐 Conversor Intergaláctico

Uma aplicação em **.NET** para converter unidades intergalácticas, calcular valores de materiais e responder perguntas em linguagem natural.

---

## 🚀 Como usar

### 1. Execute o projeto

```bash
dotnet run
```

---

## 🧭 Fluxo de uso

A aplicação funciona em 3 etapas:

---

### 🔹 1. Mapear numerais intergalácticos

Você deve associar palavras aos numerais romanos.

#### Exemplo:

```text
glob → I
prok → V
pish → X
tegj → L
```

---

### 🔹 2. Informar valores dos materiais

Agora você informa quanto um material vale em créditos.

#### Formato:

```text
<medidas> <material> é <valor> créditos
```

#### Exemplo:

```text
glob glob é é 34 créditos
```

👉 Isso significa:

* `glob glob` = 2
* então: `prata = 17 créditos por unidade`

---

### 🔹 3. Fazer perguntas

Você pode consultar valores usando linguagem natural.

---

## ❓ Tipos de perguntas

### 📌 1. Valor numérico

```text
quanto é glob glob ?
```

👉 Resultado:

```text
glob glob = 2
```

---

### 📌 2. Valor de material

```text
quanto é glob glob prata ?
```

👉 Resultado:

```text
glob glob prata = 34
```

---

## 🧪 Exemplos completos

### Entrada:

```text
glob é I
prok é V
pish é X
tegj é L

glob glob prata é 34 créditos
glob prok ouro é 57800 créditos
```

---

### Perguntas:

```text
quanto é glob glob ?
quanto é glob prok ouro ?
```

---

### Saída:

```text
glob glob = 2
glob prok ouro = 57800
```

---


## 🏗️ Tecnologias

* .NET
* C#
* xUnit (testes)

---
