import { useState } from "react";

function CurrencyConverter() {
  const [amount, setAmount] = useState("");
  const [language, setLanguage] = useState("en");
  const [result, setResult] = useState("");
  const [error, setError] = useState("");

  function handleAmountChange(e: React.ChangeEvent<HTMLInputElement>) {
    const input = e.target.value.replace(/\s/g, "");

    // Allow up to 9 digits, a comma, and 2 decimal places.
    if (!/^\d{0,9}(,\d{0,2})?$/.test(input)) {
      return;
    }

    let [integerPart, decimalPart] = input.split(",");
    integerPart = integerPart.replace(/^0+(?=\d)/, "");

    if (integerPart === "" && decimalPart !== undefined) {
      integerPart = "0";
    }

    // Add spaces as thousand separators for display (1234 -> 1 234)
    const formattedInteger = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, " ");

    const formattedAmount =
      decimalPart !== undefined
        ? `${formattedInteger},${decimalPart}`
        : formattedInteger;

    setAmount(formattedAmount);
    setResult("");
    setError("");
  }

  async function handleConvert() {
    setResult("");
    setError("");

    if (!amount || amount.endsWith(",")) {
      setError("Please enter a valid amount.");
      return;
    }

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/api/currency/convert`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            amount: amount.replace(/\s/g, ""),
            language: language,
          }),
        },
      );

      // handle unexpected HTTP errors
      if (!response.ok && response.status !== 400) {
        throw new Error("Something went wrong. Please try again.");
      }

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || "Conversion failed.");
      }

      setResult(data.result);
    } catch (error) {
      console.error("Conversion failed:", error);

      if (error instanceof SyntaxError || error instanceof TypeError) {
        setError("Something went wrong. Please try again.");
      } else if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("Something went wrong. Please try again.");
      }
    }
  }

  return (
    <div className="container py-5">
      <div className="card shadow-sm mx-auto" style={{ maxWidth: "520px" }}>
        <div className="card-body p-4">
          <h2 className="text-center mb-4">Currency Converter</h2>

          <fieldset className="mb-4">
            <legend className="form-label fs-6">Convert to</legend>

            <div className="form-check form-check-inline">
              <input
                className="form-check-input"
                type="radio"
                id="english"
                name="language"
                checked={language === "en"}
                onChange={() => setLanguage("en")}
              />
              <label className="form-check-label" htmlFor="english">
                English
              </label>
            </div>

            <div className="form-check form-check-inline">
              <input
                className="form-check-input"
                type="radio"
                id="german"
                name="language"
                checked={language === "de"}
                onChange={() => setLanguage("de")}
              />
              <label className="form-check-label" htmlFor="german">
                Deutsch
              </label>
            </div>
          </fieldset>

          <div className="mb-3">
            <label htmlFor="amount" className="form-label">
              Amount (USD)
            </label>

            <div className="input-group" style={{ width: "fit-content" }}>
              <span className="input-group-text">$</span>

              <input
                className="form-control text-end"
                id="amount"
                type="text"
                style={{
                  width: "18ch",
                  flex: "0 0 auto",
                }}
                placeholder="0,00"
                value={amount}
                onChange={handleAmountChange}
                inputMode="decimal"
              />
            </div>
          </div>

          <button
            type="button"
            className="btn btn-primary w-100 mt-3"
            onClick={handleConvert}
          >
            Convert
          </button>

          {(result || error) && (
            <div
              className={`alert ${error ? "alert-danger" : "alert-success"} mt-4 mb-0`}
              role={error ? "alert" : "status"}
            >
              {error || result}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default CurrencyConverter;
