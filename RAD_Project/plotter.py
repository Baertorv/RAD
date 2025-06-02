import pandas as pd
import matplotlib.pyplot as plt

# Load the two exported dataframes (CSV or direct input)
df_sorted = pd.read_csv("sorted_estimates.csv")
df_medians = pd.read_csv("medians.csv")
true_S = 1_000_000

# Plot 100 sorted estimates
plt.figure(figsize=(10, 5))
plt.plot(df_sorted['experiment_index'], df_sorted['estimate'], label='Sorted Estimates')
plt.axhline(y=true_S, color='red', linestyle='--', label='True S')
plt.title("Count-Sketch Estimates vs True S")
plt.xlabel("Experiment Index")
plt.ylabel("Estimated S")
plt.legend()
plt.grid(True)
plt.tight_layout()
plt.show()

# Plot 9 medians
plt.figure(figsize=(8, 4))
plt.plot(df_medians['group_index'], df_medians['median'], marker='o', label='Group Medians')
plt.axhline(y=true_S, color='red', linestyle='--', label='True S')
plt.title("Group Medians of Count-Sketch Estimates")
plt.xlabel("Group Index")
plt.ylabel("Median Estimate")
plt.legend()
plt.grid(True)
plt.tight_layout()
plt.show()