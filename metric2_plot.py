import requests
import matplotlib.pyplot as plt
import json
from matplotlib.ticker import MaxNLocator

# fetch the data
# url = "https://portkey-2a1ae-default-rtdb.firebaseio.com/playtesting1_analytics.json"
url = "https://portkey-2a1ae-default-rtdb.firebaseio.com/beta_playtesting_analytics.json"

response = requests.get(url)
data = response.json()

# parse the data
level_data = {}
for player in data.values():
    level = int(player['level']) 
    reason = player['reasonforFinshingLevel']
    
    if level not in level_data:
        level_data[level] = {'collision': 0, 'time_up': 0}
    
    if reason == 1:
        level_data[level]['collision'] += 1
    elif reason == 2:
        level_data[level]['time_up'] += 1

# helper variables
levels = sorted(level_data.keys())
collisions = [level_data[level]['collision'] for level in levels]
time_ups = [level_data[level]['time_up'] for level in levels]

# plot the data
fig, ax = plt.subplots()
bar_width = 0.4
p1 = ax.bar(levels, collisions, bar_width, label='Death')
p2 = ax.bar(levels, time_ups, bar_width, bottom=collisions, label='Time Up')

# labels and title on the graph
ax.set_xlabel('Levels')
ax.set_ylabel('Total Number of Game Completions')
ax.set_title('Level Completion Reason')
ax.set_xticks(levels) 
ax.legend()
ax.yaxis.set_major_locator(MaxNLocator(integer=True))

for level, coll, time in zip(levels, collisions, time_ups):
    if coll > 0:
            ax.text(level, coll / 2, str(coll), ha='center', va='center', color='black')
    if time > 0:
        ax.text(level, coll + time / 2, str(time), ha='center', va='center', color='black')


# display the plot
plt.show()